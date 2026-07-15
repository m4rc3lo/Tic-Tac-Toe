using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica as invariantes do objeto de valor <see cref="BoardPosition"/>.
/// </summary>
public class BoardPositionTests
{
    /// <summary>
    /// Confirma que coordenadas dentro do tabuleiro são preservadas.
    /// </summary>
    [Fact]
    public void constructor_should_preserve_valid_coordinates()
    {
        BoardPosition position = new(1, 2);

        Assert.Equal(1, position.Row);
        Assert.Equal(2, position.Column);
    }

    /// <summary>
    /// Confirma que uma linha negativa é rejeitada.
    /// </summary>
    [Fact]
    public void constructor_should_reject_negative_row()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new BoardPosition(-1, 0));
    }

    /// <summary>
    /// Confirma que uma coluna igual ao tamanho do tabuleiro é rejeitada.
    /// </summary>
    [Fact]
    public void constructor_should_reject_column_outside_board()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new BoardPosition(0, BoardPosition.BoardSize));
    }

    /// <summary>
    /// Confirma que posições com as mesmas coordenadas possuem igualdade por valor.
    /// </summary>
    [Fact]
    public void positions_with_same_coordinates_should_be_equal()
    {
        BoardPosition first_position = new(2, 1);
        BoardPosition second_position = new(2, 1);

        Assert.Equal(first_position, second_position);
    }
}
