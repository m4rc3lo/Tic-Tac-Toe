using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica que o tabuleiro exposto por <see cref="Match"/> é somente leitura.
/// </summary>
public class MatchBoardBoundaryTests
{
    /// <summary>
    /// Confirma que a visão pública não expõe o tabuleiro mutável.
    /// </summary>
    [Fact]
    public void board_property_should_expose_read_only_view()
    {
        Match match = new(
            new HumanPlayer("Ana", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));

        Assert.IsAssignableFrom<IReadOnlyBoard>(match.Board);
        Assert.IsNotType<Board>(match.Board);
    }

    /// <summary>
    /// Confirma que jogadas do agregado continuam refletidas na visão.
    /// </summary>
    [Fact]
    public void board_view_should_reflect_match_moves()
    {
        Match match = new(
            new HumanPlayer("Ana", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));
        BoardPosition position = new(1, 1);

        match.apply_move(position);

        Assert.Equal(Symbol.X, match.Board.get_symbol(position));
        Assert.Equal(1, match.Board.OccupiedCount);
    }
}
