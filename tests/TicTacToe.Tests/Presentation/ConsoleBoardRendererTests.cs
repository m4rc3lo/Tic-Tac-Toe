using TicTacToe.Domain;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

/// <summary>
/// Verifica a renderização ASCII do tabuleiro.
/// </summary>
public class ConsoleBoardRendererTests
{
    /// <summary>
    /// Confirma coordenadas, símbolos e separadores.
    /// </summary>
    [Fact]
    public void render_should_write_ascii_board()
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
        ConsoleBoardRenderer renderer = new(writer);

        renderer.render(board);

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

        Assert.Equal(expected, writer.ToString());
    }
}
