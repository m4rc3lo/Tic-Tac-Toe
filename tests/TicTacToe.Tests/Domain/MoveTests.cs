using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica as invariantes do objeto de valor <see cref="Move"/>.
/// </summary>
public class MoveTests
{
    /// <summary>
    /// Confirma que uma jogada válida preserva seus valores.
    /// </summary>
    [Fact]
    public void constructor_should_preserve_valid_move()
    {
        BoardPosition position = new(0, 1);

        Move move = new(position, Symbol.X, 1);

        Assert.Equal(position, move.Position);
        Assert.Equal(Symbol.X, move.Symbol);
        Assert.Equal(1, move.TurnNumber);
    }

    /// <summary>
    /// Confirma que uma jogada não pode utilizar o símbolo vazio.
    /// </summary>
    [Fact]
    public void constructor_should_reject_empty_symbol()
    {
        BoardPosition position = new(0, 0);

        Assert.Throws<ArgumentException>(
            () => new Move(position, Symbol.Empty, 1));
    }

    /// <summary>
    /// Confirma que a numeração dos turnos deve começar em um.
    /// </summary>
    [Fact]
    public void constructor_should_reject_zero_turn_number()
    {
        BoardPosition position = new(0, 0);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Move(position, Symbol.O, 0));
    }

    /// <summary>
    /// Confirma que jogadas equivalentes possuem igualdade por valor.
    /// </summary>
    [Fact]
    public void moves_with_same_values_should_be_equal()
    {
        Move first_move = new(new BoardPosition(2, 2), Symbol.O, 3);
        Move second_move = new(new BoardPosition(2, 2), Symbol.O, 3);

        Assert.Equal(first_move, second_move);
    }
}
