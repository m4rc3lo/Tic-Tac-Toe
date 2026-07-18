using TicTacToe.Domain;
using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class MatchPersistenceServiceTests
{
    [Fact]
    public void persist_should_save_match_and_update_statistics()
    {
        MemoryHistoryRepository history = new();
        MemoryStatisticsRepository statistics = new();
        MatchPersistenceService service = create_service(
            history,
            statistics);

        MatchRecord record = service.persist(
            create_context());

        Assert.Single(history.Records);
        Assert.Equal(record.Id, history.Records[0].Id);
        Assert.Equal(1, statistics.Value.TotalMatches);
        Assert.Equal(1, statistics.Value.XWins);
    }

    [Fact]
    public void persist_should_restore_history_when_statistics_fail()
    {
        MatchRecord previous = create_existing_record();
        MemoryHistoryRepository history = new([previous]);
        ThrowingStatisticsRepository statistics = new();
        MatchPersistenceService service = create_service(
            history,
            statistics);

        Assert.Throws<IOException>(
            () => service.persist(create_context()));

        Assert.Single(history.Records);
        Assert.Equal(previous.Id, history.Records[0].Id);
    }

    private static MatchPersistenceService create_service(
        IMatchHistoryRepository history,
        IMatchStatisticsRepository statistics)
    {
        return new MatchPersistenceService(
            history,
            statistics,
            new MatchRecordMapper(),
            new MatchStatisticsCalculator());
    }

    private static MatchPersistenceContext create_context()
    {
        Match match = new(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));
        match.apply_move(new BoardPosition(1, 1));
        match.apply_move(new BoardPosition(0, 2));

        DateTimeOffset start =
            new(2026, 7, 17, 10, 0, 0, TimeSpan.Zero);

        return new MatchPersistenceContext(
            match,
            start,
            start.AddSeconds(1),
            "Human",
            "Minimax",
            null,
            "1.7.0");
    }

    private static MatchRecord create_existing_record()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        return new MatchRecord(
            Guid.NewGuid(),
            now,
            now,
            0,
            new MatchParticipantRecord(
                "Pessoa",
                "Human",
                "X",
                null),
            new MatchParticipantRecord(
                "CPU",
                "Computer",
                "O",
                "Random"),
            Array.Empty<MatchMoveRecord>(),
            "Draw",
            Array.Empty<BoardPositionRecord>(),
            null,
            "1.7.0");
    }

    private sealed class MemoryHistoryRepository
        : IMatchHistoryRepository
    {
        public MemoryHistoryRepository(
            IReadOnlyList<MatchRecord>? records = null)
        {
            Records = records?.ToList() ?? [];
        }

        public List<MatchRecord> Records { get; private set; }

        public IReadOnlyList<MatchRecord> load_all()
        {
            return Records.AsReadOnly();
        }

        public void replace_all(
            IReadOnlyList<MatchRecord> matches)
        {
            Records = matches.ToList();
        }
    }

    private sealed class MemoryStatisticsRepository
        : IMatchStatisticsRepository
    {
        public MatchStatisticsRecord Value { get; private set; } =
            MatchStatisticsRecord.empty();

        public MatchStatisticsRecord load()
        {
            return Value;
        }

        public void save(MatchStatisticsRecord statistics)
        {
            Value = statistics;
        }
    }

    private sealed class ThrowingStatisticsRepository
        : IMatchStatisticsRepository
    {
        public MatchStatisticsRecord load()
        {
            return MatchStatisticsRecord.empty();
        }

        public void save(MatchStatisticsRecord statistics)
        {
            throw new IOException("Falha controlada.");
        }
    }
}
