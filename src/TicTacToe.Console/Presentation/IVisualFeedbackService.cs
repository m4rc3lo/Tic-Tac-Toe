using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Define realces visuais para estados relevantes do tabuleiro.
/// </summary>
public interface IVisualFeedbackService
{
    void show_last_move(
        IReadOnlyBoard board,
        BoardPosition? last_move);

    void show_winning_sequence(
        IReadOnlyBoard board,
        IReadOnlyList<BoardPosition> winning_positions);
}
