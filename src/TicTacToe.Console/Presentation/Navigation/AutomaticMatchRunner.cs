using System.Diagnostics;
using System.Reflection;
using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using TicTacToe.Persistence;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Executa uma demonstração IA contra IA usando o fluxo regular de partida.
/// </summary>
public sealed class AutomaticMatchRunner : IAutomaticMatchRunner
{
    private readonly TextWriter writer;
    private readonly IGameOutput game_output;
    private readonly IDelayService delay_service;
    private readonly PresentationPreferences preferences;
    private readonly IMoveStrategyFactory strategy_factory;
    private readonly IAutomaticModeControl mode_control;
    private readonly IMatchPersistenceService? persistence_service;

    public AutomaticMatchRunner(
        TextWriter writer,
        IGameOutput game_output,
        IDelayService delay_service,
        PresentationPreferences preferences,
        IMoveStrategyFactory strategy_factory,
        IAutomaticModeControl mode_control,
        IMatchPersistenceService? persistence_service = null)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(game_output);
        ArgumentNullException.ThrowIfNull(delay_service);
        ArgumentNullException.ThrowIfNull(preferences);
        ArgumentNullException.ThrowIfNull(strategy_factory);
        ArgumentNullException.ThrowIfNull(mode_control);

        this.writer = writer;
        this.game_output = game_output;
        this.delay_service = delay_service;
        this.preferences = preferences;
        this.strategy_factory = strategy_factory;
        this.mode_control = mode_control;
        this.persistence_service = persistence_service;
    }

    public AutomaticMatchResult run(
        AutomaticMatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        IMoveStrategy x_strategy = strategy_factory.create(
            configuration.XStrategy,
            configuration.RandomSeed);
        IMoveStrategy o_strategy = strategy_factory.create(
            configuration.OStrategy,
            configuration.RandomSeed);

        IComputerMoveStrategyResolver resolver =
            new ConfiguredComputerMoveStrategyResolver(
                new Dictionary<Symbol, IMoveStrategy>
                {
                    [Symbol.X] = x_strategy,
                    [Symbol.O] = o_strategy
                });

        IMoveSelector selector =
            new ControlledComputerMoveSelector(
                resolver,
                mode_control);
        IGameOutput delayed_output = new DelayedGameOutput(
            game_output,
            delay_service,
            preferences);
        MatchController controller = new(
            selector,
            delayed_output);
        Match match = new(
            new ComputerPlayer(
                configuration.XStrategy.ToString(),
                Symbol.X),
            new ComputerPlayer(
                configuration.OStrategy.ToString(),
                Symbol.O));

        writer.WriteLine();
        writer.WriteLine("Demonstração IA contra IA");
        writer.WriteLine($"X: {configuration.XStrategy}");
        writer.WriteLine($"O: {configuration.OStrategy}");
        writer.WriteLine(
            $"Semente: {format_seed(configuration.RandomSeed)}");
        writer.WriteLine(
            "Espaço pausa ou continua; Esc cancela.");

        DateTimeOffset started_at = DateTimeOffset.UtcNow;
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            Match completed_match = controller.run(match);
            stopwatch.Stop();

            if (configuration.PersistMatch)
            {
                persistence_service?.persist(
                    create_persistence_context(
                        completed_match,
                        configuration,
                        started_at,
                        stopwatch.Elapsed));
            }

            return new AutomaticMatchResult(
                completed_match,
                WasCancelled: false,
                configuration.RandomSeed);
        }
        catch (AutomaticModeCancelledException)
        {
            stopwatch.Stop();
            writer.WriteLine();
            writer.WriteLine("Demonstração cancelada.");

            return new AutomaticMatchResult(
                match,
                WasCancelled: true,
                configuration.RandomSeed);
        }
    }

    private static MatchPersistenceContext create_persistence_context(
        Match match,
        AutomaticMatchConfiguration configuration,
        DateTimeOffset started_at,
        TimeSpan duration)
    {
        return new MatchPersistenceContext(
            match,
            started_at,
            started_at + duration,
            configuration.XStrategy.ToString(),
            configuration.OStrategy.ToString(),
            configuration.RandomSeed,
            get_application_version());
    }

    private static string get_application_version()
    {
        Version? version = Assembly.GetExecutingAssembly()
            .GetName()
            .Version;

        return version is null
            ? "unknown"
            : $"{version.Major}.{version.Minor}.{version.Build}";
    }

    private static string format_seed(int? seed)
    {
        return seed?.ToString(
                   System.Globalization.CultureInfo.InvariantCulture)
               ?? "não definida";
    }
}
