using TicTacToe.AI;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Cria Strategies configuradas sem acoplar telas às implementações concretas.
/// </summary>
public interface IMoveStrategyFactory
{
    /// <summary>
    /// Cria uma Strategy com semente opcional.
    /// </summary>
    IMoveStrategy create(
        StrategyKind strategy_kind,
        int? random_seed);
}
