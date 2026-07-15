namespace TicTacToe.Domain;

/// <summary>
/// Representa o resultado consolidado de uma partida.
/// </summary>
public enum GameResult
{
    /// <summary>
    /// Indica que ainda não existe resultado.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indica vitória do símbolo X.
    /// </summary>
    XWins = 1,

    /// <summary>
    /// Indica vitória do símbolo O.
    /// </summary>
    OWins = 2,

    /// <summary>
    /// Indica que a partida terminou empatada.
    /// </summary>
    Draw = 3
}
