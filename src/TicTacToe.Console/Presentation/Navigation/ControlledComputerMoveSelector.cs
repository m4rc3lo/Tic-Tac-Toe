using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Seleciona jogadas computacionais com controle de pausa e cancelamento.
/// </summary>
public sealed class ControlledComputerMoveSelector : IMoveSelector
{
    private readonly IComputerMoveStrategyResolver strategy_resolver;
    private readonly IAutomaticModeControl mode_control;

    public ControlledComputerMoveSelector(
        IComputerMoveStrategyResolver strategy_resolver,
        IAutomaticModeControl mode_control)
    {
        ArgumentNullException.ThrowIfNull(strategy_resolver);
        ArgumentNullException.ThrowIfNull(mode_control);

        this.strategy_resolver = strategy_resolver;
        this.mode_control = mode_control;
    }

    public BoardPosition select_move(
        Match match,
        Player player)
    {
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(player);

        if (mode_control.wait_for_turn() ==
            AutomaticControlDecision.Cancel)
        {
            throw new AutomaticModeCancelledException();
        }

        if (player is not ComputerPlayer computer_player)
        {
            throw new NotSupportedException(
                "O modo automático aceita somente participantes computacionais.");
        }

        IMoveStrategy strategy =
            strategy_resolver.resolve_strategy(computer_player);

        return strategy.choose_move(
            match.Board,
            computer_player.Symbol);
    }
}
