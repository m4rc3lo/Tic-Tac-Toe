namespace TicTacToe.Domain;

/// <summary>
/// Representa o estado geral do ciclo de vida de uma partida.
/// </summary>
public enum GameState
{
    /// <summary>
    /// Indica que a partida ainda não foi iniciada.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Indica que a partida está em andamento.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Indica que a partida foi encerrada.
    /// </summary>
    Finished = 2
}
