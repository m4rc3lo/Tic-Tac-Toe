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
            persistence_service: null)
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
            persistence_service: null)
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
        IMatchPersistenceService? persistence_service)
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
    }

    /// <inheritdoc />
    public Match play(MatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        IMoveStrategy strategy =
            create_strategy(
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

        persistence_service?.persist(
            new MatchPersistenceContext(
                completed_match,
                started_at,
                started_at + stopwatch.Elapsed,
                "Human",
                configuration.OpponentStrategy.ToString(),
                configuration.RandomSeed,
                get_application_version()));

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

    private static IMoveStrategy create_strategy(
        StrategyKind strategy_kind,
        int? random_seed)
    {
        return strategy_kind switch
        {
            StrategyKind.Random =>
                random_seed.HasValue
                    ? new RandomMoveStrategy(random_seed.Value)
                    : new RandomMoveStrategy(),
            StrategyKind.Heuristic =>
                random_seed.HasValue
                    ? new HeuristicMoveStrategy(random_seed.Value)
                    : new HeuristicMoveStrategy(),
            StrategyKind.Minimax => new MinimaxMoveStrategy(),
            _ => throw new ArgumentOutOfRangeException(
                nameof(strategy_kind),
                strategy_kind,
                "A estratégia selecionada não é suportada.")
        };
    }
}
