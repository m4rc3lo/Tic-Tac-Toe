namespace TicTacToe.Persistence;

/// <summary>
/// Representa uma jogada persistida.
/// </summary>
public sealed record MatchMoveRecord(
    int TurnNumber,
    int Row,
    int Column,
    string Symbol);
