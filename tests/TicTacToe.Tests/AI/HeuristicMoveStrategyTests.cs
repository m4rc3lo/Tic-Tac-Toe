using TicTacToe.AI;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.AI;

/// <summary>
/// Verifica prioridades, validade e reprodutibilidade da estratégia heurística.
/// </summary>
public class HeuristicMoveStrategyTests
{
    /// <summary>
    /// Confirma que uma vitória imediata tem prioridade máxima.
    /// </summary>
    [Fact]
    public void choose_move_should_prioritize_immediate_win()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (0, 1, Symbol.X),
            (1, 0, Symbol.O),
            (1, 1, Symbol.O));
        HeuristicMoveStrategy strategy = new(1);

        BoardPosition position = strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(0, 2), position);
    }

    /// <summary>
    /// Confirma que a vitória é escolhida antes de um bloqueio possível.
    /// </summary>
    [Fact]
    public void choose_move_should_choose_win_before_block()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (0, 1, Symbol.X),
            (1, 0, Symbol.O),
            (1, 1, Symbol.O));
        HeuristicMoveStrategy strategy = new(1);

        BoardPosition position = strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(0, 2), position);
        Assert.NotEqual(new BoardPosition(1, 2), position);
    }

    /// <summary>
    /// Confirma o bloqueio de uma vitória imediata do adversário.
    /// </summary>
    [Fact]
    public void choose_move_should_prioritize_block_after_win()
    {
        Board board = create_board(
            (0, 0, Symbol.O),
            (0, 1, Symbol.O),
            (1, 1, Symbol.X));
        HeuristicMoveStrategy strategy = new(1);

        BoardPosition position = strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(0, 2), position);
    }

    /// <summary>
    /// Confirma a preferência pelo centro quando não há tática imediata.
    /// </summary>
    [Fact]
    public void choose_move_should_prioritize_center()
    {
        Board board = create_board(
            (0, 0, Symbol.O));
        HeuristicMoveStrategy strategy = new(1);

        BoardPosition position = strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(1, 1), position);
    }

    /// <summary>
    /// Confirma a preferência por cantos quando o centro está ocupado.
    /// </summary>
    [Fact]
    public void choose_move_should_prioritize_corner_after_center()
    {
        Board board = create_board(
            (1, 1, Symbol.O));
        HeuristicMoveStrategy strategy = new(
            new FixedRandomSource(2));

        BoardPosition position = strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(2, 0), position);
    }

    /// <summary>
    /// Confirma a escolha de uma lateral quando centro e cantos estão ocupados.
    /// </summary>
    [Fact]
    public void choose_move_should_choose_side_after_corners()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (0, 2, Symbol.O),
            (1, 0, Symbol.O),
            (1, 1, Symbol.X),
            (1, 2, Symbol.X),
            (2, 0, Symbol.X),
            (2, 1, Symbol.O),
            (2, 2, Symbol.O));
        HeuristicMoveStrategy strategy = new(1);

        BoardPosition position = strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(0, 1), position);
    }

    /// <summary>
    /// Confirma o desempate reproduzível entre múltiplas vitórias.
    /// </summary>
    [Fact]
    public void same_seed_should_reproduce_winning_tie_break()
    {
        Board board = create_board(
            (1, 0, Symbol.X),
            (1, 1, Symbol.X),
            (0, 2, Symbol.X),
            (1, 2, Symbol.O),
            (2, 2, Symbol.X),
            (0, 1, Symbol.O),
            (2, 1, Symbol.O));
        HeuristicMoveStrategy first_strategy = new(2026);
        HeuristicMoveStrategy second_strategy = new(2026);

        BoardPosition first_position =
            first_strategy.choose_move(board, Symbol.X);
        BoardPosition second_position =
            second_strategy.choose_move(board, Symbol.X);

        Assert.Equal(first_position, second_position);
        Assert.Contains(
            first_position,
            new[]
            {
                new BoardPosition(0, 0),
                new BoardPosition(2, 0)
            });
    }

    /// <summary>
    /// Confirma que a posição escolhida está disponível.
    /// </summary>
    [Fact]
    public void choose_move_should_return_available_position()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (1, 1, Symbol.O),
            (2, 2, Symbol.X));
        HeuristicMoveStrategy strategy = new(42);

        BoardPosition position = strategy.choose_move(board, Symbol.O);

        Assert.True(board.is_position_available(position));
    }

    /// <summary>
    /// Confirma que a avaliação não modifica o tabuleiro recebido.
    /// </summary>
    [Fact]
    public void choose_move_should_preserve_original_board()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (0, 1, Symbol.X),
            (1, 1, Symbol.O));
        Symbol[,] before = capture_symbols(board);
        int occupied_count = board.OccupiedCount;
        HeuristicMoveStrategy strategy = new(42);

        strategy.choose_move(board, Symbol.O);

        Assert.Equal(occupied_count, board.OccupiedCount);
        assert_symbols_equal(before, board);
    }

    /// <summary>
    /// Confirma a rejeição de um tabuleiro completo.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_full_board()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (0, 1, Symbol.O),
            (0, 2, Symbol.X),
            (1, 0, Symbol.X),
            (1, 1, Symbol.O),
            (1, 2, Symbol.O),
            (2, 0, Symbol.O),
            (2, 1, Symbol.X),
            (2, 2, Symbol.X));
        HeuristicMoveStrategy strategy = new(1);

        Assert.Throws<InvalidOperationException>(
            () => strategy.choose_move(board, Symbol.X));
    }

    /// <summary>
    /// Confirma a rejeição do símbolo vazio.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_empty_symbol()
    {
        HeuristicMoveStrategy strategy = new(1);

        Assert.Throws<ArgumentException>(
            () => strategy.choose_move(new Board(), Symbol.Empty));
    }

    /// <summary>
    /// Confirma que um índice inválido da fonte injetada é detectado.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_invalid_random_index()
    {
        Board board = create_board(
            (1, 1, Symbol.X));
        HeuristicMoveStrategy strategy = new(
            new FixedRandomSource(4));

        Assert.Throws<InvalidOperationException>(
            () => strategy.choose_move(board, Symbol.O));
    }

    private static Board create_board(
        params (int Row, int Column, Symbol Symbol)[] cells)
    {
        Board board = new();
        int turn_number = 1;

        foreach ((int row, int column, Symbol symbol) in cells)
        {
            board.apply_move(
                new Move(
                    new BoardPosition(row, column),
                    symbol,
                    turn_number));

            turn_number++;
        }

        return board;
    }

    private static Symbol[,] capture_symbols(IReadOnlyBoard board)
    {
        Symbol[,] symbols =
            new Symbol[BoardPosition.BoardSize, BoardPosition.BoardSize];

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                symbols[row, column] =
                    board.get_symbol(new BoardPosition(row, column));
            }
        }

        return symbols;
    }

    private static void assert_symbols_equal(
        Symbol[,] expected,
        IReadOnlyBoard board)
    {
        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                Assert.Equal(
                    expected[row, column],
                    board.get_symbol(new BoardPosition(row, column)));
            }
        }
    }

    private sealed class FixedRandomSource : IRandomSource
    {
        private readonly int value;

        public FixedRandomSource(int value)
        {
            this.value = value;
        }

        public int next(int max_exclusive)
        {
            return value;
        }
    }
}
