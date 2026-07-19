using TicTacToe.AI;

namespace TicTacToe.Application;

/// <summary>
/// Cria as Strategies concretas disponíveis no experimento.
/// </summary>
public sealed class ExperimentStrategyFactory : IExperimentStrategyFactory
{
    public IMoveStrategy create(ExperimentStrategy strategy, int? seed)
    {
        return strategy switch
        {
            ExperimentStrategy.Random => seed.HasValue
                ? new RandomMoveStrategy(seed.Value)
                : new RandomMoveStrategy(),
            ExperimentStrategy.Heuristic => seed.HasValue
                ? new HeuristicMoveStrategy(seed.Value)
                : new HeuristicMoveStrategy(),
            ExperimentStrategy.Minimax => new MinimaxMoveStrategy(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };
    }
}
