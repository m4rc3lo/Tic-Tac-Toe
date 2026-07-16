using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Define a porta de saída utilizada pelo fluxo de aplicação.
/// </summary>
public interface IGameOutput
{
    /// <summary>
    /// Apresenta o estado atual da partida.
    /// </summary>
    /// <param name="match">Partida a ser apresentada.</param>
    void show_match(Match match);

    /// <summary>
    /// Comunica que a posição selecionada não pôde ser aplicada.
    /// </summary>
    /// <param name="player">Participante responsável pela tentativa.</param>
    /// <param name="position">Posição rejeitada.</param>
    /// <param name="message">Descrição compreensível do problema.</param>
    void show_invalid_move(
        Player player,
        BoardPosition position,
        string message);

    /// <summary>
    /// Apresenta o resultado final da partida.
    /// </summary>
    /// <param name="match">Partida encerrada.</param>
    void show_result(Match match);
}
