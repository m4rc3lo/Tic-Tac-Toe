using TicTacToe.AI;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.AI;

/// <summary>
/// Verifica decisões, métricas e isolamento da estratégia Minimax.
/// </summary>
public class MinimaxMoveStrategyTests
{
    /// <summary>
    /// Confirma que uma vitória imediata é escolhida.
    /// </summary>
    [Fact]
    public void choose_move_should_select_immediate_win()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (0, 1, Symbol.X),
            (1, 0, Symbol.O),
            (1, 1, Symbol.O));

        MinimaxMoveStrategy strategy = new();

        BoardPosition position =
            strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(0, 2), position);
    }

    /// <summary>
    /// Confirma o bloqueio de uma vitória adversária obrigatória.
    /// </summary>
    [Fact]
    public void choose_move_should_block_required_opponent_win()
    {
        Board board = create_board(
            (0, 0, Symbol.O),
            (0, 1, Symbol.O),
            (1, 0, Symbol.X));

        MinimaxMoveStrategy strategy = new();

        BoardPosition position =
            strategy.choose_move(board, Symbol.X);

        Assert.Equal(new BoardPosition(0, 2), position);
    }

    /// <summary>
    /// Confirma empate quando dois agentes Minimax jogam perfeitamente.
    /// </summary>
    [Fact]
    public void perfect_play_should_finish_with_draw()
    {
        Board board = new();
        MinimaxMoveStrategy strategy = new();
        Symbol current_symbol = Symbol.X;
        int turn_number = 1;

        while (!board.IsFull &&
               GameRules.evaluate(board).IsInProgress)
        {
            BoardPosition position =
                strategy.choose_move(board, current_symbol);

            board.apply_move(
                new Move(position, current_symbol, turn_number));

            current_symbol = current_symbol == Symbol.X
                ? Symbol.O
                : Symbol.X;

            turn_number++;
        }

        GameEvaluation evaluation = GameRules.evaluate(board);

        Assert.Equal(GameResult.Draw, evaluation.Result);
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

        MinimaxMoveStrategy strategy = new();

        BoardPosition position =
            strategy.choose_move(board, Symbol.O);

        Assert.True(board.is_position_available(position));
    }

    /// <summary>
    /// Confirma que a análise não modifica o tabuleiro recebido.
    /// </summary>
    [Fact]
    public void analyze_should_preserve_original_board()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (1, 1, Symbol.O),
            (2, 0, Symbol.X));
        Symbol[,] before = capture_symbols(board);
        int occupied_count = board.OccupiedCount;

        MinimaxMoveStrategy strategy = new();

        strategy.analyze(board, Symbol.O);

        Assert.Equal(occupied_count, board.OccupiedCount);

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                Assert.Equal(
                    before[row, column],
                    board.get_symbol(
                        new BoardPosition(row, column)));
            }
        }
    }

    /// <summary>
    /// Confirma que estados iguais produzem análise idêntica.
    /// </summary>
    [Fact]
    public void analyze_should_be_reproducible()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (1, 1, Symbol.O));

        MinimaxMoveStrategy first_strategy = new();
        MinimaxMoveStrategy second_strategy = new();

        MinimaxAnalysis first_analysis =
            first_strategy.analyze(board, Symbol.X);
        MinimaxAnalysis second_analysis =
            second_strategy.analyze(board, Symbol.X);

        Assert.Equal(first_analysis, second_analysis);
    }

    /// <summary>
    /// Confirma o registro de estados e profundidade.
    /// </summary>
    [Fact]
    public void analyze_should_report_search_metrics()
    {
        Board board = create_board(
            (0, 0, Symbol.X),
            (1, 1, Symbol.O));

        MinimaxMoveStrategy strategy = new();

        MinimaxAnalysis analysis =
            strategy.analyze(board, Symbol.X);

        Assert.True(analysis.VisitedStates > 0);
        Assert.True(analysis.MaximumDepth > 0);
    }

    /// <summary>
    /// Confirma que o resultado da análise não é uma jogada do domínio.
    /// </summary>
    [Fact]
    public void analyze_should_return_analysis_separate_from_move()
    {
        Board board = new();
        MinimaxMoveStrategy strategy = new();

        MinimaxAnalysis analysis =
            strategy.analyze(board, Symbol.X);

        Assert.IsType<MinimaxAnalysis>(analysis);
        Assert.True(board.is_position_available(analysis.Position));
    }

    /// <summary>
    /// Confirma a rejeição de tabuleiro completo.
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

        MinimaxMoveStrategy strategy = new();

        Assert.Throws<InvalidOperationException>(
            () => strategy.choose_move(board, Symbol.O));
    }

    /// <summary>
    /// Confirma a rejeição do símbolo vazio.
    /// </summary>
    [Fact]
    public void choose_move_should_reject_empty_symbol()
    {
        MinimaxMoveStrategy strategy = new();

        Assert.Throws<ArgumentException>(
            () => strategy.choose_move(
                new Board(),
                Symbol.Empty));
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
                    board.get_symbol(
                        new BoardPosition(row, column));
            }
        }

        return symbols;
    }
}
