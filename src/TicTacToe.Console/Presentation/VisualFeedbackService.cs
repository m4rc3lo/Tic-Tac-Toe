using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Implementa realces por meio do renderizador de tabuleiro.
/// </summary>
public sealed class VisualFeedbackService : IVisualFeedbackService
{
    private readonly ConsoleBoardRenderer board_renderer;

    public VisualFeedbackService(
        ConsoleBoardRenderer board_renderer)
    {
        ArgumentNullException.ThrowIfNull(board_renderer);
        this.board_renderer = board_renderer;
    }

    public void show_last_move(
        IReadOnlyBoard board,
        BoardPosition? last_move)
    {
        board_renderer.render(
            board,
            last_move,
            Array.Empty<BoardPosition>());
    }

    public void show_winning_sequence(
        IReadOnlyBoard board,
        IReadOnlyList<BoardPosition> winning_positions)
    {
        ArgumentNullException.ThrowIfNull(winning_positions);

        board_renderer.render(
            board,
            last_move: null,
            winning_positions);
    }
}
