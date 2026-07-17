using TicTacToe.AI;
using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Resolve a Strategy para um participante computacional.
/// </summary>
public interface IComputerMoveStrategyResolver
{
    /// <summary>
    /// Obtém a estratégia configurada para o participante.
    /// </summary>
    /// <param name="player">Participante computacional.</param>
    /// <returns>Estratégia associada.</returns>
    IMoveStrategy resolve_strategy(ComputerPlayer player);
}
