
namespace TicTacToe.Persistence;

/// <summary>
/// Representa uma partida concluída em formato independente do domínio.
/// </summary>
public sealed record MatchRecord
{
    private IReadOnlyList<MatchMoveRecord> moves =
        Array.Empty<MatchMoveRecord>();
    private IReadOnlyList<BoardPositionRecord> winning_sequence =
        Array.Empty<BoardPositionRecord>();

    public MatchRecord()
    {
    }

    public MatchRecord(
        Guid id,
        DateTimeOffset started_at,
        DateTimeOffset finished_at,
        long duration_milliseconds,
        MatchParticipantRecord first_participant,
        MatchParticipantRecord second_participant,
        IEnumerable<MatchMoveRecord> moves,
        string result,
        IEnumerable<BoardPositionRecord> winning_sequence,
        int? random_seed,
        string application_version)
    {
        Id = id;
        StartedAt = started_at;
        FinishedAt = finished_at;
        DurationMilliseconds = duration_milliseconds;
        FirstParticipant = first_participant;
        SecondParticipant = second_participant;
        Moves = moves.ToArray();
        Result = result;
        WinningSequence = winning_sequence.ToArray();
        RandomSeed = random_seed;
        ApplicationVersion = application_version;
    }

    public Guid Id { get; init; }
    public DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset FinishedAt { get; init; }
    public long DurationMilliseconds { get; init; }
    public MatchParticipantRecord FirstParticipant { get; init; } = null!;
    public MatchParticipantRecord SecondParticipant { get; init; } = null!;

    public IReadOnlyList<MatchMoveRecord> Moves
    {
        get => moves;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            moves = Array.AsReadOnly(value.ToArray());
        }
    }

    public string Result { get; init; } = string.Empty;

    public IReadOnlyList<BoardPositionRecord> WinningSequence
    {
        get => winning_sequence;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            winning_sequence = Array.AsReadOnly(value.ToArray());
        }
    }

    public int? RandomSeed { get; init; }
    public string ApplicationVersion { get; init; } = string.Empty;
}
