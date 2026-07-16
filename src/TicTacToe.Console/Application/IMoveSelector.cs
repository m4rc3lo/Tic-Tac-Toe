using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Define a porta responsável por selecionar uma posição conforme o tipo de
/// participante.
/// </summary>
public interface IMoveSelector
{
    /// <summary>
    /// Seleciona uma posição para o participante atual.
    /// </summary>
    /// <param name="match">Partida em andamento.</param>
    /// <param name="player">Participante responsável pelo turno.</param>
    /// <returns>Posição selecionada.</returns>
    BoardPosition select_move(Match match, Player player);
}
