using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica o comportamento e as invariantes de <see cref="Board"/>.
/// </summary>
public class BoardTests
{
    /// <summary>
    /// Confirma que um novo tabuleiro inicia vazio.
    /// </summary>
    [Fact]
    public void constructor_should_create_empty_board()
    {
        Board board = new();

        Assert.Equal(0, board.OccupiedCount);
        Assert.False(board.IsFull);
        Assert.Equal(9, board.get_available_positions().Count);
    }

    /// <summary>
    /// Confirma que todas as casas de um novo tabuleiro contêm o símbolo vazio.
    /// </summary>
    [Fact]
    public void constructor_should_initialize_every_position_as_empty()
    {
        Board board = new();

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0; column < BoardPosition.BoardSize; column++)
            {
                BoardPosition position = new(row, column);

                Assert.Equal(Symbol.Empty, board.get_symbol(position));
                Assert.True(board.is_position_available(position));
            }
        }
    }

    /// <summary>
    /// Confirma que uma jogada válida ocupa a posição indicada.
    /// </summary>
    [Fact]
    public void apply_move_should_store_symbol_in_position()
    {
        Board board = new();
        Move move = new(new BoardPosition(1, 2), Symbol.X, 1);

        board.apply_move(move);

        Assert.Equal(Symbol.X, board.get_symbol(move.Position));
        Assert.False(board.is_position_available(move.Position));
        Assert.Equal(1, board.OccupiedCount);
    }

    /// <summary>
    /// Confirma que aplicar jogadas distintas atualiza a contagem de ocupação.
    /// </summary>
    [Fact]
    public void apply_move_should_increment_occupied_count()
    {
        Board board = new();

        board.apply_move(new Move(new BoardPosition(0, 0), Symbol.X, 1));
        board.apply_move(new Move(new BoardPosition(2, 2), Symbol.O, 2));

        Assert.Equal(2, board.OccupiedCount);
        Assert.Equal(7, board.get_available_positions().Count);
    }

    /// <summary>
    /// Confirma que uma casa ocupada não pode receber uma segunda jogada.
    /// </summary>
    [Fact]
    public void apply_move_should_reject_occupied_position()
    {
        Board board = new();
        BoardPosition position = new(1, 1);

        board.apply_move(new Move(position, Symbol.X, 1));

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => board.apply_move(new Move(position, Symbol.O, 2)));

        Assert.Contains("já está ocupada", exception.Message);
        Assert.Equal(Symbol.X, board.get_symbol(position));
        Assert.Equal(1, board.OccupiedCount);
    }

    /// <summary>
    /// Confirma que a coleção de casas livres é ordenada por linha e coluna.
    /// </summary>
    [Fact]
    public void get_available_positions_should_return_row_major_order()
    {
        Board board = new();
        board.apply_move(new Move(new BoardPosition(0, 1), Symbol.X, 1));

        IReadOnlyList<BoardPosition> positions = board.get_available_positions();

        Assert.Equal(
            [
                new BoardPosition(0, 0),
                new BoardPosition(0, 2),
                new BoardPosition(1, 0),
                new BoardPosition(1, 1),
                new BoardPosition(1, 2),
                new BoardPosition(2, 0),
                new BoardPosition(2, 1),
                new BoardPosition(2, 2)
            ],
            positions);
    }

    /// <summary>
    /// Confirma que a coleção retornada não expõe o armazenamento interno.
    /// </summary>
    [Fact]
    public void get_available_positions_should_return_independent_collection()
    {
        Board board = new();

        IReadOnlyList<BoardPosition> first_result = board.get_available_positions();
        board.apply_move(new Move(new BoardPosition(0, 0), Symbol.X, 1));
        IReadOnlyList<BoardPosition> second_result = board.get_available_positions();

        Assert.Equal(9, first_result.Count);
        Assert.Equal(8, second_result.Count);
    }

    /// <summary>
    /// Confirma que desfazer uma jogada restaura a casa vazia.
    /// </summary>
    [Fact]
    public void undo_move_should_restore_empty_position()
    {
        Board board = new();
        BoardPosition position = new(2, 0);
        board.apply_move(new Move(position, Symbol.O, 1));

        Symbol removed_symbol = board.undo_move(position);

        Assert.Equal(Symbol.O, removed_symbol);
        Assert.Equal(Symbol.Empty, board.get_symbol(position));
        Assert.True(board.is_position_available(position));
        Assert.Equal(0, board.OccupiedCount);
    }

    /// <summary>
    /// Confirma que não é possível desfazer uma casa vazia.
    /// </summary>
    [Fact]
    public void undo_move_should_reject_empty_position()
    {
        Board board = new();
        BoardPosition position = new(0, 2);

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => board.undo_move(position));

        Assert.Contains("já está vazia", exception.Message);
        Assert.Equal(0, board.OccupiedCount);
    }

    /// <summary>
    /// Confirma que um tabuleiro com nove jogadas é reconhecido como completo.
    /// </summary>
    [Fact]
    public void is_full_should_be_true_after_nine_moves()
    {
        Board board = create_full_board();

        Assert.True(board.IsFull);
        Assert.Equal(9, board.OccupiedCount);
        Assert.Empty(board.get_available_positions());
    }

    /// <summary>
    /// Confirma que desfazer uma jogada torna um tabuleiro completo novamente disponível.
    /// </summary>
    [Fact]
    public void undo_move_should_make_full_board_available_again()
    {
        Board board = create_full_board();
        BoardPosition position = new(2, 2);

        board.undo_move(position);

        Assert.False(board.IsFull);
        Assert.Equal(8, board.OccupiedCount);
        Assert.Single(board.get_available_positions());
        Assert.Equal(position, board.get_available_positions()[0]);
    }

    /// <summary>
    /// Confirma que aplicar e desfazer repetidamente preserva a contagem interna.
    /// </summary>
    [Fact]
    public void apply_and_undo_should_preserve_occupied_count_consistency()
    {
        Board board = new();
        BoardPosition position = new(1, 0);

        board.apply_move(new Move(position, Symbol.X, 1));
        board.undo_move(position);
        board.apply_move(new Move(position, Symbol.O, 2));

        Assert.Equal(1, board.OccupiedCount);
        Assert.Equal(Symbol.O, board.get_symbol(position));
    }

    private static Board create_full_board()
    {
        Board board = new();
        int turn_number = 1;

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0; column < BoardPosition.BoardSize; column++)
            {
                Symbol symbol = turn_number % 2 == 0
                    ? Symbol.O
                    : Symbol.X;

                board.apply_move(
                    new Move(
                        new BoardPosition(row, column),
                        symbol,
                        turn_number));

                turn_number++;
            }
        }

        return board;
    }
}
