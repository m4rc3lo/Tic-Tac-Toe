using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class MatchStatisticsCalculatorTests
{
    [Fact]
    public void calculate_should_aggregate_results_moves_duration_and_strategy()
    {
        MatchRecord first =
            create_record("XWins", 5, 100, "Minimax");
        MatchRecord second =
            create_record("Draw", 9, 300, "Minimax");

        MatchStatisticsRecord statistics =
            new MatchStatisticsCalculator().calculate(
                [first, second]);

        Assert.Equal(2, statistics.TotalMatches);
        Assert.Equal(1, statistics.XWins);
        Assert.Equal(1, statistics.Draws);
        Assert.Equal(14, statistics.TotalMoves);
        Assert.Equal(7, statistics.AverageMoves);
        Assert.Equal(200, statistics.AverageDurationMilliseconds);

        StrategyStatisticsRecord strategy =
            Assert.Single(statistics.Strategies);

        Assert.Equal("Minimax", strategy.Strategy);
        Assert.Equal(2, strategy.Matches);
        Assert.Equal(1, strategy.Losses);
        Assert.Equal(1, strategy.Draws);
    }

    private static MatchRecord create_record(
        string result,
        int move_count,
        long duration,
        string strategy)
    {
        MatchMoveRecord[] moves =
            Enumerable.Range(1, move_count)
                .Select(turn =>
                    new MatchMoveRecord(
                        turn,
                        0,
                        0,
                        turn % 2 == 0 ? "O" : "X"))
                .ToArray();

        return new MatchRecord(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            duration,
            new MatchParticipantRecord(
                "Pessoa",
                "Human",
                "X",
                null),
            new MatchParticipantRecord(
                "CPU",
                "Computer",
                "O",
                strategy),
            moves,
            result,
            Array.Empty<BoardPositionRecord>(),
            null,
            "1.7.0");
    }
}
