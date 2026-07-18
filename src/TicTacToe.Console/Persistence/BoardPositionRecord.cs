namespace TicTacToe.Persistence;

/// <summary>
/// Representa uma posição persistida do tabuleiro.
/// </summary>
public sealed record BoardPositionRecord(
    int Row,
    int Column);
