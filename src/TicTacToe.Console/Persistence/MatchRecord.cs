namespace TicTacToe.Persistence;

/// <summary>
/// Representa uma partida concluída em formato independente do domínio.
/// </summary>
public sealed record MatchRecord(
    Guid Id,
    DateTimeOffset StartedAt,
    DateTimeOffset FinishedAt,
    long DurationMilliseconds,
    MatchParticipantRecord FirstParticipant,
    MatchParticipantRecord SecondParticipant,
    IReadOnlyList<MatchMoveRecord> Moves,
    string Result,
    IReadOnlyList<BoardPositionRecord> WinningSequence,
    int? RandomSeed,
    string ApplicationVersion);
