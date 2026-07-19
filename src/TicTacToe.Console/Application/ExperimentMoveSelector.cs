using TicTacToe.AI;
using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Seleciona jogadas computacionais e acumula métricas opcionais.
/// </summary>
public sealed class ExperimentMoveSelector : IMoveSelector
{
    private readonly IComputerMoveStrategyResolver resolver;

    public ExperimentMoveSelector(IComputerMoveStrategyResolver resolver)
    {
        ArgumentNullException.ThrowIfNull(resolver);
        this.resolver = resolver;
    }

    public long EvaluatedStates { get; private set; }

    public BoardPosition select_move(Match match, Player player)
    {
        if (player is not ComputerPlayer computer_player)
        {
            throw new InvalidOperationException(
                "O modo experimental aceita somente participantes computacionais.");
        }

        IMoveStrategy strategy = resolver.resolve_strategy(computer_player);
        BoardPosition position = strategy.choose_move(match.Board, player.Symbol);

        if (strategy is ISearchMetricsProvider metrics_provider)
        {
            EvaluatedStates += metrics_provider.LastEvaluatedStates;
        }

        return position;
    }
}
