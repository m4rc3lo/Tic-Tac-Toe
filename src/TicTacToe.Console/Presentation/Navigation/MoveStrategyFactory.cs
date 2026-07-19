using TicTacToe.AI;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Cria as Strategies disponíveis na aplicação.
/// </summary>
public sealed class MoveStrategyFactory : IMoveStrategyFactory
{
    /// <inheritdoc />
    public IMoveStrategy create(
        StrategyKind strategy_kind,
        int? random_seed)
    {
        return strategy_kind switch
        {
            StrategyKind.Random =>
                random_seed.HasValue
                    ? new RandomMoveStrategy(random_seed.Value)
                    : new RandomMoveStrategy(),
            StrategyKind.Heuristic =>
                random_seed.HasValue
                    ? new HeuristicMoveStrategy(random_seed.Value)
                    : new HeuristicMoveStrategy(),
            StrategyKind.Minimax => new MinimaxMoveStrategy(),
            _ => throw new ArgumentOutOfRangeException(
                nameof(strategy_kind),
                strategy_kind,
                "A Strategy selecionada não é suportada.")
        };
    }
}
