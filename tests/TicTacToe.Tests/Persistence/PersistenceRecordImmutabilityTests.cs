
using TicTacToe.Application;
using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class PersistenceRecordImmutabilityTests
{
    [Fact]
    public void match_record_should_copy_collection_inputs()
    {
        List<MatchMoveRecord> moves =
        [
            new MatchMoveRecord(1, 0, 0, "X")
        ];
        List<BoardPositionRecord> winning =
        [
            new BoardPositionRecord(0, 0)
        ];

        MatchRecord record = new(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            1,
            new MatchParticipantRecord(
                "A",
                "Computer",
                "X",
                "Minimax"),
            new MatchParticipantRecord(
                "B",
                "Computer",
                "O",
                "Random"),
            moves,
            "XWins",
            winning,
            1,
            "1.8.0");

        moves.Clear();
        winning.Clear();

        Assert.Single(record.Moves);
        Assert.Single(record.WinningSequence);
    }

    [Fact]
    public void statistics_record_should_copy_strategy_input()
    {
        List<StrategyStatisticsRecord> strategies =
        [
            new StrategyStatisticsRecord(
                "Minimax",
                1,
                1,
                0,
                0)
        ];

        MatchStatisticsRecord record = new(
            1,
            1,
            0,
            0,
            5,
            5,
            1,
            strategies);

        strategies.Clear();

        Assert.Single(record.Strategies);
    }

    [Fact]
    public void experiment_result_should_copy_metrics_input()
    {
        List<ExperimentMetricRecord> metrics =
        [
            new ExperimentMetricRecord(
                Guid.NewGuid(),
                1,
                "Random",
                "Minimax",
                10,
                "Draw",
                9,
                1,
                null,
                false,
                null,
                "1.8.0")
        ];
        ExperimentConfiguration configuration = new(
            ExperimentStrategy.Random,
            ExperimentStrategy.Minimax,
            1,
            false,
            false,
            10,
            "1.8.0",
            "exports");

        ExperimentResult result = new(
            Guid.NewGuid(),
            configuration,
            metrics,
            new ExperimentSummary(
                1,
                1,
                0,
                0,
                0,
                1,
                9,
                1));

        metrics.Clear();

        Assert.Single(result.Metrics);
    }
}
