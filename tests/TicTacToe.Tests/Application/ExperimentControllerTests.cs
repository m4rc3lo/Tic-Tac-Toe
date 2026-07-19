using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Application;

public class ExperimentControllerTests
{
    [Fact]
    public void run_should_be_reproducible_and_aggregate_results()
    {
        MemoryRepository repository = new();
        ExperimentController controller = create_controller(
            new ExperimentStrategyFactory(),
            repository);

        ExperimentResult result = controller.run(
            create_configuration(match_count: 4));

        Assert.Equal(4, result.Metrics.Count);
        Assert.Equal(
            new int?[] { 10, 11, 12, 13 },
            result.Metrics.Select(metric => metric.Seed).ToArray());
        Assert.Equal(4, result.Summary.SuccessfulRuns);
        Assert.Equal(4, repository.SaveCount);
        Assert.All(result.Metrics, metric => Assert.False(metric.Failed));
    }

    [Fact]
    public void run_should_alternate_symbols_and_first_player()
    {
        MemoryRepository repository = new();
        RecordingFactory factory = new();
        ExperimentController controller = create_controller(factory, repository);

        ExperimentResult result = controller.run(create_configuration(match_count: 2));

        Assert.Equal("Random", result.Metrics[0].XStrategy);
        Assert.Equal("Minimax", result.Metrics[0].OStrategy);
        Assert.Equal("Minimax", result.Metrics[1].XStrategy);
        Assert.Equal("Random", result.Metrics[1].OStrategy);
        Assert.Equal(4, factory.Calls.Count);
    }

    [Fact]
    public void run_should_capture_failure_and_continue()
    {
        MemoryRepository repository = new();
        ExperimentController controller = create_controller(
            new FailingOnceFactory(),
            repository);

        ExperimentResult result = controller.run(create_configuration(match_count: 2));

        Assert.Equal(2, result.Metrics.Count);
        Assert.True(result.Metrics[0].Failed);
        Assert.False(result.Metrics[1].Failed);
        Assert.Equal(1, result.Summary.FailedRuns);
        Assert.Equal(2, repository.SaveCount);
    }

    [Fact]
    public void run_should_collect_optional_search_metrics()
    {
        MemoryRepository repository = new();
        ExperimentController controller = create_controller(
            new MetricsFactory(),
            repository);

        ExperimentResult result = controller.run(create_configuration(match_count: 1));

        Assert.NotNull(result.Metrics[0].EvaluatedStates);
        Assert.True(result.Metrics[0].EvaluatedStates > 0);
    }

    private static ExperimentController create_controller(
        IExperimentStrategyFactory factory,
        IExperimentResultRepository repository)
    {
        return new ExperimentController(
            factory,
            new SequentialSeedSequence(),
            new FakeTimerFactory(),
            [repository]);
    }

    private static ExperimentConfiguration create_configuration(int match_count)
    {
        return new ExperimentConfiguration(
            ExperimentStrategy.Random,
            ExperimentStrategy.Minimax,
            match_count,
            AlternateSymbols: true,
            AlternateFirstPlayer: true,
            BaseSeed: 10,
            ApplicationVersion: "1.8.0",
            OutputDirectory: "ignored");
    }

    private sealed class MemoryRepository : IExperimentResultRepository
    {
        public int SaveCount { get; private set; }
        public void save(string output_directory, ExperimentResult result) => SaveCount++;
    }

    private sealed class FakeTimerFactory : IExperimentTimerFactory
    {
        public IExperimentTimer start() => new FakeTimer();
    }

    private sealed class FakeTimer : IExperimentTimer
    {
        public DateTimeOffset StartedAt { get; } = DateTimeOffset.UnixEpoch;
        public DateTimeOffset FinishedAt { get; private set; } = DateTimeOffset.UnixEpoch;
        public long ElapsedMilliseconds => 25;
        public void stop() => FinishedAt = StartedAt.AddMilliseconds(25);
    }

    private sealed class RecordingFactory : IExperimentStrategyFactory
    {
        private readonly ExperimentStrategyFactory inner = new();
        public List<ExperimentStrategy> Calls { get; } = [];
        public IMoveStrategy create(ExperimentStrategy strategy, int? seed)
        {
            Calls.Add(strategy);
            return inner.create(strategy, seed);
        }
    }

    private sealed class FailingOnceFactory : IExperimentStrategyFactory
    {
        private int calls;
        private readonly ExperimentStrategyFactory inner = new();
        public IMoveStrategy create(ExperimentStrategy strategy, int? seed)
        {
            calls++;
            if (calls == 1) return new ThrowingStrategy();
            return inner.create(strategy, seed);
        }
    }

    private sealed class ThrowingStrategy : IMoveStrategy
    {
        public BoardPosition choose_move(IReadOnlyBoard board, Symbol symbol) =>
            throw new InvalidOperationException("Falha controlada.");
    }

    private sealed class MetricsFactory : IExperimentStrategyFactory
    {
        public IMoveStrategy create(ExperimentStrategy strategy, int? seed) =>
            new MetricsStrategy();
    }

    private sealed class MetricsStrategy : IMoveStrategy, ISearchMetricsProvider
    {
        public long LastEvaluatedStates { get; private set; }
        public BoardPosition choose_move(IReadOnlyBoard board, Symbol symbol)
        {
            LastEvaluatedStates = 3;
            return board.get_available_positions()[0];
        }
    }
}
