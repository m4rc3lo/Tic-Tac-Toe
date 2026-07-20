
using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class MatchPersistenceRecoveryServiceTests
{
    [Fact]
    public void recover_should_recalculate_statistics_from_history()
    {
        MatchRecord record = create_record();
        MemoryHistoryRepository history = new([record]);
        MemoryStatisticsRepository statistics = new();

        MatchStatisticsRecord recovered =
            new MatchPersistenceRecoveryService(
                history,
                statistics,
                new MatchStatisticsCalculator())
            .recover();

        Assert.Equal(1, recovered.TotalMatches);
        Assert.Equal(recovered, statistics.Value);
    }

    private static MatchRecord create_record()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        return new MatchRecord(
            Guid.NewGuid(),
            now,
            now,
            0,
            new MatchParticipantRecord("A", "Computer", "X", "Minimax"),
            new MatchParticipantRecord("B", "Computer", "O", "Random"),
            Array.Empty<MatchMoveRecord>(),
            "Draw",
            Array.Empty<BoardPositionRecord>(),
            1,
            "1.8.0");
    }

    private sealed class MemoryHistoryRepository
        : IMatchHistoryRepository
    {
        private readonly IReadOnlyList<MatchRecord> records;

        public MemoryHistoryRepository(
            IReadOnlyList<MatchRecord> records)
        {
            this.records = records;
        }

        public IReadOnlyList<MatchRecord> load_all() => records;

        public void replace_all(
            IReadOnlyList<MatchRecord> matches)
        {
        }
    }

    private sealed class MemoryStatisticsRepository
        : IMatchStatisticsRepository
    {
        public MatchStatisticsRecord Value { get; private set; } =
            MatchStatisticsRecord.empty();

        public MatchStatisticsRecord load() => Value;

        public void save(MatchStatisticsRecord statistics)
        {
            Value = statistics;
        }
    }
}
