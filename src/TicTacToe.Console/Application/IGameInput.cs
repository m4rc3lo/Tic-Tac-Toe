using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Define a porta de entrada utilizada para obter jogadas de participantes
/// humanos.
/// </summary>
public interface IGameInput
{
    /// <summary>
    /// Obtém uma posição para o participante informado.
    /// </summary>
    /// <param name="match">Partida em andamento.</param>
    /// <param name="player">Pessoa responsável pelo turno.</param>
    /// <returns>Posição selecionada.</returns>
    BoardPosition read_move(Match match, HumanPlayer player);
}
