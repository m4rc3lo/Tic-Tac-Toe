using TicTacToe.Domain;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public class VisualFeedbackServiceTests
{
    [Fact]
    public void show_winning_sequence_should_render_without_changing_board()
    {
        Board board = new();
        board.apply_move(
            new Move(
                new BoardPosition(0, 0),
                Symbol.X,
                1));
        board.apply_move(
            new Move(
                new BoardPosition(0, 1),
                Symbol.X,
                2));
        board.apply_move(
            new Move(
                new BoardPosition(0, 2),
                Symbol.X,
                3));

        StringWriter writer = new();
        ConsoleBoardRenderer renderer = new(
            writer,
            new ConsoleTheme(
                new PresentationPreferences(
                    use_ansi_colors: true,
                    visual_effects: true)));
        VisualFeedbackService service = new(renderer);

        service.show_winning_sequence(
            board,
            [
                new BoardPosition(0, 0),
                new BoardPosition(0, 1),
                new BoardPosition(0, 2)
            ]);

        Assert.Contains("\u001b[32mX\u001b[0m", writer.ToString());
        Assert.Equal(3, board.OccupiedCount);
    }
    [Fact]
    public void show_last_move_should_use_ascii_marker_without_colors()
    {
        Board board = new();
        BoardPosition position = new(1, 1);
        board.apply_move(
            new Move(
                position,
                Symbol.O,
                1));

        StringWriter writer = new();
        ConsoleBoardRenderer renderer = new(
            writer,
            new ConsoleTheme(
                new PresentationPreferences(
                    use_ansi_colors: false,
                    use_unicode: false,
                    visual_effects: true)));
        VisualFeedbackService service = new(renderer);

        service.show_last_move(board, position);

        Assert.Contains("[O]", writer.ToString());
    }

}
