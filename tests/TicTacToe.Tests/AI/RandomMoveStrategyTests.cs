using TicTacToe.AI;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.AI;

/// <summary>
/// Verifica validade, injeção e reprodutibilidade da estratégia aleatória.
/// </summary>
public class RandomMoveStrategyTests
{
    /// <summary>
    /// Confirma que a estratégia seleciona uma casa disponível.
    /// </summary>
    [Fact]
    public void choose_move_should_return_available_position()
    {
        Board board = new();
        board.apply_move(new Move(new BoardPosition(0, 0), Symbol.X, 1));
        board.apply_move(new Move(new BoardPosition(1, 1), Symbol.O, 2));

        RandomMoveStrategy strategy = new(new FixedRandomSource(0));

        BoardPosition selected_position = strategy.choose_move(board, Symbol.X);

        Assert.True(board.is_position_available(selected_position));
        Assert.NotEqual(new BoardPosition(0, 0), selected_position);
        Assert.NotEqual(new BoardPosition(1, 1), selected_position);
    }

    /// <summary>
    /// Confirma que a posição selecionada corresponde ao índice injetado.
    /// </summary>
    [Fact]
    public void choose_move_should_use_injected_random_source()
    {
        Board board = new();
        RandomMoveStrategy strategy = new(new FixedRandomSource(4));

        BoardPosition selected_position = strategy.choose_move(board, Symbol.O);

        Assert.Equal(new BoardPosition(1, 1), selected_position);
    }

    /// <summary>
    /// Confirma que geradores com a mesma semente produzem a mesma sequência.
    /// </summary>
    [Fact]
    public void same_seed_should_produce_same_sequence()
    {
        RandomMoveStrategy first_strategy = new(2026);
        RandomMoveStrategy second_strategy = new(2026);

        List<BoardPosition> first_sequence = create_sequence(first_strategy);
        List<BoardPosition> second_sequence = create_sequence(second_strategy);

        Assert.Equal(first_sequence, second_sequence);
    }

    /// <summary>
    /// Confirma que a estratégia não modifica o tabuleiro.
    /// </summary>
    [Fact]
    public void choose_move_should_not_modify_board()
    {
        Board board = new();
        board.apply_move(new Move(new BoardPosition(2, 2), Symbol.X, 1));
        int occupied_count = board.OccupiedCount;

        RandomMoveStrategy strategy = new(10);

        strategy.choose_move(board, Symbol.O);

        Assert.Equal(occupied_count, board.OccupiedCount);
        Assert.Equal(Symbol.X, board.get_symbol(new BoardPosition(2, 2)));
    }

    /// <summary>
    /// Confirma que um tabuleiro completo é rejeitado.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_full_board()
    {
        Board board = create_full_board();
        RandomMoveStrategy strategy = new(1);

        Assert.Throws<InvalidOperationException>(
            () => strategy.choose_move(board, Symbol.X));
    }

    /// <summary>
    /// Confirma que o símbolo vazio é rejeitado.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_empty_symbol()
    {
        RandomMoveStrategy strategy = new(1);

        Assert.Throws<ArgumentException>(
            () => strategy.choose_move(new Board(), Symbol.Empty));
    }

    /// <summary>
    /// Confirma que índices inválidos do gerador injetado são detectados.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_invalid_random_index()
    {
        RandomMoveStrategy strategy = new(new FixedRandomSource(9));

        Assert.Throws<InvalidOperationException>(
            () => strategy.choose_move(new Board(), Symbol.X));
    }

    private static List<BoardPosition> create_sequence(
        RandomMoveStrategy strategy)
    {
        Board board = new();
        List<BoardPosition> positions = [];

        for (int turn_number = 1; turn_number <= 5; turn_number++)
        {
            Symbol symbol = turn_number % 2 == 1
                ? Symbol.X
                : Symbol.O;

            BoardPosition position = strategy.choose_move(board, symbol);
            positions.Add(position);
            board.apply_move(new Move(position, symbol, turn_number));
        }

        return positions;
    }

    private static Board create_full_board()
    {
        Board board = new();
        int turn_number = 1;

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0; column < BoardPosition.BoardSize; column++)
            {
                Symbol symbol = turn_number % 2 == 1
                    ? Symbol.X
                    : Symbol.O;

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
