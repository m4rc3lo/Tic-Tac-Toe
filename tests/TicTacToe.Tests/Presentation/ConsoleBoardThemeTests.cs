using TicTacToe.Domain;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

/// <summary>
/// Verifica saídas exatas dos modos ASCII e Unicode.
/// </summary>
public class ConsoleBoardThemeTests
{
    [Fact]
    public void render_should_write_exact_ascii_board()
    {
        string actual = render(use_unicode: false);

        string expected = string.Join(
            Environment.NewLine,
            [
                "    1   2   3",
                "1   X |   |  ",
                "   ---+---+---",
                "2     | O |  ",
                "   ---+---+---",
                "3     |   |  ",
                string.Empty
            ]);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void render_should_write_exact_unicode_board()
    {
        string actual = render(use_unicode: true);

        string expected = string.Join(
            Environment.NewLine,
            [
                "    1   2   3",
                "1   X │   │  ",
                "   ───┼───┼───",
                "2     │ O │  ",
                "   ───┼───┼───",
                "3     │   │  ",
                string.Empty
            ]);

        Assert.Equal(expected, actual);
    }

    private static string render(bool use_unicode)
    {
        Board board = new();
        board.apply_move(
            new Move(
                new BoardPosition(0, 0),
                Symbol.X,
                1));
        board.apply_move(
            new Move(
                new BoardPosition(1, 1),
                Symbol.O,
                2));

        StringWriter writer = new();
        PresentationPreferences preferences = new(
            use_unicode: use_unicode,
            visual_effects: false);
        ConsoleBoardRenderer renderer = new(
            writer,
            new ConsoleTheme(preferences));

        renderer.render(board);

        return writer.ToString();
    }
}
