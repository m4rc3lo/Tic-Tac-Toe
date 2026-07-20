using System.Diagnostics;
using System.Reflection;
using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using TicTacToe.Persistence;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Compõe e executa uma partida usando os adaptadores textuais existentes.
/// </summary>
public sealed class ConsoleMatchSessionRunner : IMatchSessionRunner
{
    private readonly IGameInput game_input;
    private readonly IGameOutput game_output;
    private readonly IAnimationService animation_service;
    private readonly PresentationPreferences preferences;
    private readonly IMatchPersistenceService? persistence_service;
    private readonly IMoveStrategyFactory strategy_factory;
    private readonly IExternalFailureReporter failure_reporter;

    /// <summary>
    /// Inicializa o executor de sessões sem persistência externa.
    /// </summary>
    public ConsoleMatchSessionRunner(
        IGameInput game_input,
        IGameOutput game_output)
        : this(
            game_input,
            game_output,
            new AnimationService(
                TextWriter.Null,
                new ImmediateDelayService(),
                new PresentationPreferences(
                    visual_effects: false)),
            new PresentationPreferences(
                visual_effects: false),
            persistence_service: null,
            strategy_factory: new MoveStrategyFactory(),
            failure_reporter: new NullExternalFailureReporter())
    {
    }

    /// <summary>
    /// Inicializa o executor de sessões sem persistência externa.
    /// </summary>
    public ConsoleMatchSessionRunner(
        IGameInput game_input,
        IGameOutput game_output,
        IAnimationService animation_service,
        PresentationPreferences preferences)
        : this(
            game_input,
            game_output,
            animation_service,
            preferences,
            persistence_service: null,
            strategy_factory: new MoveStrategyFactory(),
            failure_reporter: new NullExternalFailureReporter())
    {
    }

    /// <summary>
    /// Inicializa o executor de sessões com persistência opcional.
    /// </summary>
    public ConsoleMatchSessionRunner(
        IGameInput game_input,
        IGameOutput game_output,
        IAnimationService animation_service,
        PresentationPreferences preferences,
        IMatchPersistenceService? persistence_service,
        IMoveStrategyFactory? strategy_factory = null,
        IExternalFailureReporter? failure_reporter = null)
    {
        ArgumentNullException.ThrowIfNull(game_input);
        ArgumentNullException.ThrowIfNull(game_output);
        ArgumentNullException.ThrowIfNull(animation_service);
        ArgumentNullException.ThrowIfNull(preferences);

        this.game_input = game_input;
        this.game_output = game_output;
        this.animation_service = animation_service;
        this.preferences = preferences;
        this.persistence_service = persistence_service;
        this.strategy_factory = strategy_factory ?? new MoveStrategyFactory();
        this.failure_reporter = failure_reporter ??
            new NullExternalFailureReporter();
    }

    /// <inheritdoc />
    public Match play(MatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        IMoveStrategy strategy =
            strategy_factory.create(
                configuration.OpponentStrategy,
                configuration.RandomSeed);

        IComputerMoveStrategyResolver strategy_resolver =
            new ConfiguredComputerMoveStrategyResolver(
                new Dictionary<Symbol, IMoveStrategy>
                {
                    [Symbol.O] = strategy
                });

        IMoveSelector move_selector =
            new AnimatedMoveSelector(
                new DefaultMoveSelector(
                    game_input,
                    strategy_resolver),
                animation_service,
                preferences);

        MatchController controller = new(
            move_selector,
            game_output);

        Match match = new(
            new HumanPlayer(
                configuration.PlayerName,
                Symbol.X),
            new ComputerPlayer(
                configuration.OpponentStrategy.ToString(),
                Symbol.O));

        DateTimeOffset started_at =
            DateTimeOffset.UtcNow;
        Stopwatch stopwatch = Stopwatch.StartNew();

        Match completed_match =
            controller.run(match);

        stopwatch.Stop();

        try
        {
            persistence_service?.persist(
                new MatchPersistenceContext(
                    completed_match,
                    started_at,
                    started_at + stopwatch.Elapsed,
                    "Human",
                    configuration.OpponentStrategy.ToString(),
                    configuration.RandomSeed,
                    get_application_version()));
        }
        catch (InfrastructureOperationException exception)
        {
            failure_reporter.report(
                "A partida não pôde ser registrada.",
                exception);
        }

        return completed_match;
    }

    private static string get_application_version()
    {
        Version? version =
            Assembly.GetExecutingAssembly()
                .GetName()
                .Version;

        return version is null
            ? "unknown"
            : $"{version.Major}.{version.Minor}.{version.Build}";
    }
}
