using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica vitória, empate, partida em andamento e falsos positivos.
/// </summary>
public class GameRulesTests
{
    /// <summary>
    /// Confirma a detecção de vitória em qualquer linha.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void evaluate_should_detect_row_win(int row)
    {
        BoardPosition[] positions =
        [
            new BoardPosition(row, 0),
            new BoardPosition(row, 1),
            new BoardPosition(row, 2)
        ];

        Board board = create_board_with_sequence(Symbol.X, positions);

        GameEvaluation evaluation = GameRules.evaluate(board);

        Assert.Equal(GameResult.XWins, evaluation.Result);
        Assert.Equal(positions, evaluation.WinningPositions);
    }

    /// <summary>
    /// Confirma a detecção de vitória em qualquer coluna.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void evaluate_should_detect_column_win(int column)
    {
        BoardPosition[] positions =
        [
            new BoardPosition(0, column),
            new BoardPosition(1, column),
            new BoardPosition(2, column)
        ];

        Board board = create_board_with_sequence(Symbol.O, positions);

        GameEvaluation evaluation = GameRules.evaluate(board);

        Assert.Equal(GameResult.OWins, evaluation.Result);
        Assert.Equal(positions, evaluation.WinningPositions);
    }

    /// <summary>
    /// Confirma a detecção da diagonal principal.
    /// </summary>
    [Fact]
    public void evaluate_should_detect_main_diagonal_win()
    {
        BoardPosition[] positions =
        [
            new BoardPosition(0, 0),
            new BoardPosition(1, 1),
            new BoardPosition(2, 2)
        ];

        GameEvaluation evaluation = GameRules.evaluate(
            create_board_with_sequence(Symbol.X, positions));

        Assert.Equal(GameResult.XWins, evaluation.Result);
        Assert.Equal(positions, evaluation.WinningPositions);
    }

    /// <summary>
    /// Confirma a detecção da diagonal secundária.
    /// </summary>
    [Fact]
    public void evaluate_should_detect_secondary_diagonal_win()
    {
        BoardPosition[] positions =
        [
            new BoardPosition(0, 2),
            new BoardPosition(1, 1),
            new BoardPosition(2, 0)
        ];

        GameEvaluation evaluation = GameRules.evaluate(
            create_board_with_sequence(Symbol.O, positions));

        Assert.Equal(GameResult.OWins, evaluation.Result);
        Assert.Equal(positions, evaluation.WinningPositions);
    }

    /// <summary>
    /// Confirma que um tabuleiro completo sem vencedor é empate.
    /// </summary>
    [Fact]
    public void evaluate_should_detect_draw()
    {
        Board board = create_board(
            [
                Symbol.X, Symbol.O, Symbol.X,
                Symbol.X, Symbol.O, Symbol.O,
                Symbol.O, Symbol.X, Symbol.X
            ]);

        GameEvaluation evaluation = GameRules.evaluate(board);

        Assert.Equal(GameResult.Draw, evaluation.Result);
        Assert.False(evaluation.HasWinner);
        Assert.False(evaluation.IsInProgress);
        Assert.Empty(evaluation.WinningPositions);
    }

    /// <summary>
    /// Confirma que um tabuleiro vazio representa partida em andamento.
    /// </summary>
    [Fact]
    public void evaluate_should_report_empty_board_as_in_progress()
    {
        GameEvaluation evaluation = GameRules.evaluate(new Board());

        Assert.Equal(GameResult.None, evaluation.Result);
        Assert.True(evaluation.IsInProgress);
        Assert.Empty(evaluation.WinningPositions);
    }

    /// <summary>
    /// Confirma que um tabuleiro parcial sem vencedor permanece em andamento.
    /// </summary>
    [Fact]
    public void evaluate_should_report_partial_board_as_in_progress()
    {
        Board board = create_board(
            [
                Symbol.X, Symbol.O, Symbol.Empty,
                Symbol.Empty, Symbol.X, Symbol.Empty,
                Symbol.O, Symbol.Empty, Symbol.Empty
            ]);

        Assert.True(GameRules.evaluate(board).IsInProgress);
    }

    /// <summary>
    /// Confirma que duas marcas e uma casa vazia não formam vitória.
    /// </summary>
    [Fact]
    public void evaluate_should_not_report_incomplete_line_as_win()
    {
        Board board = create_board(
            [
                Symbol.X, Symbol.X, Symbol.Empty,
                Symbol.O, Symbol.Empty, Symbol.Empty,
                Symbol.Empty, Symbol.O, Symbol.Empty
            ]);

        Assert.Equal(GameResult.None, GameRules.evaluate(board).Result);
    }

    /// <summary>
    /// Confirma que símbolos mistos não formam vitória.
    /// </summary>
    [Fact]
    public void evaluate_should_not_report_mixed_line_as_win()
    {
        Board board = create_board(
            [
                Symbol.X, Symbol.O, Symbol.X,
                Symbol.Empty, Symbol.Empty, Symbol.Empty,
                Symbol.Empty, Symbol.Empty, Symbol.Empty
            ]);

        Assert.Equal(GameResult.None, GameRules.evaluate(board).Result);
    }

    /// <summary>
    /// Confirma que uma sequência vazia não forma vitória.
    /// </summary>
    [Fact]
    public void evaluate_should_not_report_empty_sequence_as_win()
    {
        Board board = create_board(
            [
                Symbol.X, Symbol.O, Symbol.Empty,
                Symbol.O, Symbol.X, Symbol.Empty,
                Symbol.Empty, Symbol.Empty, Symbol.Empty
            ]);

        Assert.Equal(GameResult.None, GameRules.evaluate(board).Result);
    }

    /// <summary>
    /// Confirma que a avaliação não modifica o tabuleiro.
    /// </summary>
    [Fact]
    public void evaluate_should_not_modify_board()
    {
        Board board = create_board(
            [
                Symbol.X, Symbol.X, Symbol.X,
                Symbol.O, Symbol.O, Symbol.Empty,
                Symbol.Empty, Symbol.Empty, Symbol.Empty
            ]);

        Symbol[] before = snapshot(board);

        GameRules.evaluate(board);

        Assert.Equal(before, snapshot(board));
        Assert.Equal(5, board.OccupiedCount);
    }

    /// <summary>
    /// Confirma que um tabuleiro nulo é rejeitado.
    /// </summary>
    [Fact]
    public void evaluate_should_reject_null_board()
    {
        Assert.Throws<ArgumentNullException>(
            () => GameRules.evaluate(null!));
    }

    private static Board create_board_with_sequence(
        Symbol symbol,
        IReadOnlyList<BoardPosition> positions)
    {
        Board board = new();
        int turn_number = 1;

        foreach (BoardPosition position in positions)
        {
            board.apply_move(new Move(position, symbol, turn_number));
            turn_number++;
        }

        return board;
    }

    private static Board create_board(IReadOnlyList<Symbol> symbols)
    {
        Board board = new();
        int turn_number = 1;

        for (int index = 0; index < symbols.Count; index++)
        {
            Symbol symbol = symbols[index];

            if (symbol == Symbol.Empty)
            {
                continue;
            }

            board.apply_move(
                new Move(
                    new BoardPosition(
                        index / BoardPosition.BoardSize,
                        index % BoardPosition.BoardSize),
                    symbol,
                    turn_number));

            turn_number++;
        }

        return board;
    }

    private static Symbol[] snapshot(Board board)
    {
        List<Symbol> symbols = [];

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0; column < BoardPosition.BoardSize; column++)
            {
                symbols.Add(board.get_symbol(new BoardPosition(row, column)));
            }
        }

        return symbols.ToArray();
    }
}
