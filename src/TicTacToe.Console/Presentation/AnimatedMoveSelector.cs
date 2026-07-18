using TicTacToe.Application;
using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Decora a seleção de jogadas com feedback antes de decisões computacionais.
/// </summary>
public sealed class AnimatedMoveSelector : IMoveSelector
{
    private readonly IMoveSelector inner_selector;
    private readonly IAnimationService animation_service;

    public AnimatedMoveSelector(
        IMoveSelector inner_selector,
        IAnimationService animation_service)
    {
        ArgumentNullException.ThrowIfNull(inner_selector);
        ArgumentNullException.ThrowIfNull(animation_service);

        this.inner_selector = inner_selector;
        this.animation_service = animation_service;
    }

    public BoardPosition select_move(
        Match match,
        Player player)
    {
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(player);

        if (player is ComputerPlayer)
        {
            animation_service.show_analysis_indicator(
                $"{player.Name} está analisando",
                frame_count: 4,
                frame_delay: TimeSpan.FromMilliseconds(80));
        }

        return inner_selector.select_move(match, player);
    }
}
