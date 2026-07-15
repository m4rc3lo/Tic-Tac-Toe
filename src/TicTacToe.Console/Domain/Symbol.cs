namespace TicTacToe.Domain;

/// <summary>
/// Representa os símbolos possíveis em uma casa do tabuleiro.
/// </summary>
public enum Symbol
{
    /// <summary>
    /// Indica que a casa não contém uma jogada.
    /// </summary>
    Empty = 0,

    /// <summary>
    /// Representa o símbolo X.
    /// </summary>
    X = 1,

    /// <summary>
    /// Representa o símbolo O.
    /// </summary>
    O = 2
}
