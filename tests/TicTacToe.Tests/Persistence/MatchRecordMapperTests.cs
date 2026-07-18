using TicTacToe.Domain;
using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class MatchRecordMapperTests
{
    [Fact]
    public void map_should_create_complete_immutable_record()
    {
        Match match = create_finished_match();
        DateTimeOffset started_at =
            new(2026, 7, 17, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset finished_at =
            started_at.AddMilliseconds(250);

        MatchRecord record =
            new MatchRecordMapper().map(
                new MatchPersistenceContext(
                    match,
                    started_at,
                    finished_at,
                    "Human",
                    "Minimax",
                    2026,
                    "1.7.0"));

        Assert.NotEqual(Guid.Empty, record.Id);
        Assert.Equal(250, record.DurationMilliseconds);
        Assert.Equal("Pessoa", record.FirstParticipant.Name);
        Assert.Equal("Minimax", record.SecondParticipant.Strategy);
        Assert.Equal(5, record.Moves.Count);
        Assert.Equal("XWins", record.Result);
        Assert.Equal(3, record.WinningSequence.Count);
        Assert.Equal(2026, record.RandomSeed);
        Assert.Equal("1.7.0", record.ApplicationVersion);
    }

    private static Match create_finished_match()
    {
        Match match = new(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));
        match.apply_move(new BoardPosition(1, 1));
        match.apply_move(new BoardPosition(0, 2));

        return match;
    }
}
