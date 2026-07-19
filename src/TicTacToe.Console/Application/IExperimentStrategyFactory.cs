using TicTacToe.AI;

namespace TicTacToe.Application;

/// <summary>
/// Cria Strategies para uma execução experimental.
/// </summary>
public interface IExperimentStrategyFactory
{
    IMoveStrategy create(ExperimentStrategy strategy, int? seed);
}
