namespace TicTacToe.Persistence;

/// <summary>
/// Representa um participante persistido sem referência às entidades do domínio.
/// </summary>
public sealed record MatchParticipantRecord(
    string Name,
    string Kind,
    string Symbol,
    string? Strategy);
