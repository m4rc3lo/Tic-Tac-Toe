using TicTacToe.AI;
using TicTacToe.Domain;
using TicTacToe.Persistence;

namespace TicTacToe.Application;

/// <summary>
/// Executa confrontos em lote sem dependências de apresentação.
/// </summary>
public sealed class ExperimentController
{
    private readonly IExperimentStrategyFactory strategy_factory;
    private readonly ISeedSequence seed_sequence;
    private readonly IExperimentTimerFactory timer_factory;
    private readonly IReadOnlyList<IExperimentResultRepository> repositories;
    private readonly IMatchPersistenceService? match_persistence_service;
    private readonly IExperimentInfrastructureReporter infrastructure_reporter;

    public ExperimentController(
        IExperimentStrategyFactory strategy_factory,
        ISeedSequence seed_sequence,
        IExperimentTimerFactory timer_factory,
        IEnumerable<IExperimentResultRepository> repositories,
        IMatchPersistenceService? match_persistence_service = null,
        IExperimentInfrastructureReporter? infrastructure_reporter = null)
    {
        ArgumentNullException.ThrowIfNull(strategy_factory);
        ArgumentNullException.ThrowIfNull(seed_sequence);
        ArgumentNullException.ThrowIfNull(timer_factory);
        ArgumentNullException.ThrowIfNull(repositories);

        this.strategy_factory = strategy_factory;
        this.seed_sequence = seed_sequence;
        this.timer_factory = timer_factory;
        this.repositories = repositories.ToArray();
        this.match_persistence_service = match_persistence_service;
        this.infrastructure_reporter = infrastructure_reporter ??
            new NullExperimentInfrastructureReporter();
    }

    public ExperimentResult run(ExperimentConfiguration configuration)
    {
        validate(configuration);

        Guid experiment_id = Guid.NewGuid();
        List<ExperimentMetricRecord> metrics = [];

        for (int run_number = 1;
             run_number <= configuration.MatchCount;
             run_number++)
        {
            int? seed = seed_sequence.get_seed(
                configuration.BaseSeed,
                run_number);
            (ExperimentStrategy x_kind, ExperimentStrategy o_kind) =
                resolve_strategy_assignment(configuration, run_number);

            try
            {
                ExecutionOutcome outcome = execute_run(
                    configuration,
                    experiment_id,
                    run_number,
                    seed,
                    x_kind,
                    o_kind);
                metrics.Add(outcome.Metric);

                if (configuration.PersistMatchesToHistory)
                {
                    persist_match(configuration, outcome, seed);
                }
            }
            catch (Exception exception)
            {
                metrics.Add(create_failure_metric(
                    configuration,
                    experiment_id,
                    run_number,
                    seed,
                    x_kind,
                    o_kind,
                    exception));

                save_progress(configuration, experiment_id, metrics);

                if (!configuration.ContinueOnFailure)
                {
                    break;
                }

                continue;
            }

            save_progress(configuration, experiment_id, metrics);
        }

        return create_result(configuration, experiment_id, metrics);
    }

    private ExecutionOutcome execute_run(
        ExperimentConfiguration configuration,
        Guid experiment_id,
        int run_number,
        int? seed,
        ExperimentStrategy x_kind,
        ExperimentStrategy o_kind)
    {
        IMoveStrategy x_strategy = strategy_factory.create(x_kind, seed);
        IMoveStrategy o_strategy = strategy_factory.create(o_kind, seed);

        IComputerMoveStrategyResolver resolver =
            new ConfiguredComputerMoveStrategyResolver(
                new Dictionary<Symbol, IMoveStrategy>
                {
                    [Symbol.X] = x_strategy,
                    [Symbol.O] = o_strategy
                });

        ExperimentMoveSelector selector = new(resolver);
        MatchController controller = new(selector, new NullGameOutput());

        ComputerPlayer x_player = new(x_kind.ToString(), Symbol.X);
        ComputerPlayer o_player = new(o_kind.ToString(), Symbol.O);
        bool o_starts =
            configuration.AlternateFirstPlayer && run_number % 2 == 0;
        Match match = o_starts
            ? new Match(o_player, x_player)
            : new Match(x_player, o_player);

        IExperimentTimer timer = timer_factory.start();
        Match completed_match;

        try
        {
            completed_match = controller.run(match);
        }
        finally
        {
            timer.stop();
        }

        ExperimentMetricRecord metric = new(
            experiment_id,
            run_number,
            x_kind.ToString(),
            o_kind.ToString(),
            seed,
            completed_match.Result.ToString(),
            completed_match.Moves.Count,
            timer.ElapsedMilliseconds,
            selector.EvaluatedStates > 0
                ? selector.EvaluatedStates
                : null,
            false,
            null,
            configuration.ApplicationVersion);

        return new ExecutionOutcome(
            completed_match,
            metric,
            timer.StartedAt,
            timer.FinishedAt,
            x_kind,
            o_kind);
    }

    private void persist_match(
        ExperimentConfiguration configuration,
        ExecutionOutcome outcome,
        int? seed)
    {
        match_persistence_service?.persist(
            new MatchPersistenceContext(
                outcome.Match,
                outcome.StartedAt,
                outcome.FinishedAt,
                outcome.XStrategy.ToString(),
                outcome.OStrategy.ToString(),
                seed,
                configuration.ApplicationVersion));
    }

    private void save_progress(
        ExperimentConfiguration configuration,
        Guid experiment_id,
        IReadOnlyList<ExperimentMetricRecord> metrics)
    {
        ExperimentResult result = create_result(
            configuration,
            experiment_id,
            metrics);

        foreach (IExperimentResultRepository repository in repositories)
        {
            try
            {
                repository.save(
                    configuration.OutputDirectory,
                    result);
            }
            catch (Exception exception)
                when (is_infrastructure_failure(exception))
            {
                infrastructure_reporter.report(
                    repository.GetType().Name,
                    exception);
            }
        }
    }

    private static bool is_infrastructure_failure(
        Exception exception)
    {
        return exception is InfrastructureOperationException
            or IOException
            or UnauthorizedAccessException;
    }

    private static ExperimentResult create_result(
        ExperimentConfiguration configuration,
        Guid experiment_id,
        IReadOnlyList<ExperimentMetricRecord> metrics)
    {
        ExperimentMetricRecord[] snapshot = metrics.ToArray();
        ExperimentMetricRecord[] successful = snapshot
            .Where(metric => !metric.Failed)
            .ToArray();

        ExperimentSummary summary = new(
            snapshot.Length,
            successful.Length,
            snapshot.Count(metric => metric.Failed),
            successful.Count(metric => metric.Result == "XWins"),
            successful.Count(metric => metric.Result == "OWins"),
            successful.Count(metric => metric.Result == "Draw"),
            successful.Length == 0
                ? 0
                : successful.Average(metric => metric.MoveCount),
            successful.Length == 0
                ? 0
                : successful.Average(metric => metric.DurationMilliseconds));

        return new ExperimentResult(
            experiment_id,
            configuration,
            snapshot,
            summary);
    }

    private static ExperimentMetricRecord create_failure_metric(
        ExperimentConfiguration configuration,
        Guid experiment_id,
        int run_number,
        int? seed,
        ExperimentStrategy x_kind,
        ExperimentStrategy o_kind,
        Exception exception)
    {
        return new ExperimentMetricRecord(
            experiment_id,
            run_number,
            x_kind.ToString(),
            o_kind.ToString(),
            seed,
            "Failed",
            0,
            0,
            null,
            true,
            exception.Message,
            configuration.ApplicationVersion);
    }

    private static (
        ExperimentStrategy XStrategy,
        ExperimentStrategy OStrategy)
        resolve_strategy_assignment(
            ExperimentConfiguration configuration,
            int run_number)
    {
        bool swap_symbols =
            configuration.AlternateSymbols && run_number % 2 == 0;

        return swap_symbols
            ? (configuration.OStrategy, configuration.XStrategy)
            : (configuration.XStrategy, configuration.OStrategy);
    }

    private static void validate(ExperimentConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (configuration.MatchCount < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(configuration),
                "A quantidade de partidas deve ser maior que zero.");
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ApplicationVersion);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.OutputDirectory);
    }

    private sealed record ExecutionOutcome(
        Match Match,
        ExperimentMetricRecord Metric,
        DateTimeOffset StartedAt,
        DateTimeOffset FinishedAt,
        ExperimentStrategy XStrategy,
        ExperimentStrategy OStrategy);
}
