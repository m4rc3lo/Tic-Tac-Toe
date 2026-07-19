using TicTacToe.Domain;

namespace TicTacToe.AI;

/// <summary>
/// Seleciona jogadas por busca Minimax completa.
/// </summary>
/// <remarks>
/// A estratégia copia o tabuleiro para um <see cref="SearchBoard"/> e alterna
/// níveis de maximização e minimização até estados terminais. Empates de
/// pontuação são resolvidos pela primeira posição em ordem linha-coluna.
/// </remarks>
public sealed class MinimaxMoveStrategy : IMoveStrategy, ISearchMetricsProvider
{
    /// <inheritdoc />
    public long LastEvaluatedStates { get; private set; }
    private const int WinScore = 10;
    private const int LossScore = -10;
    private const int DrawScore = 0;

    /// <inheritdoc />
    public BoardPosition choose_move(
        IReadOnlyBoard board,
        Symbol symbol)
    {
        MinimaxAnalysis analysis = analyze(board, symbol);
        LastEvaluatedStates = analysis.VisitedStates;
        return analysis.Position;
    }

    /// <summary>
    /// Analisa o estado e retorna a melhor posição acompanhada das métricas da
    /// busca.
    /// </summary>
    /// <param name="board">Tabuleiro consultado.</param>
    /// <param name="symbol">Símbolo maximizado.</param>
    /// <returns>Resultado completo da análise.</returns>
    /// <exception cref="ArgumentNullException">
    /// Lançada quando o tabuleiro é nulo.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada quando o símbolo é vazio.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando não existem posições disponíveis.
    /// </exception>
    public MinimaxAnalysis analyze(
        IReadOnlyBoard board,
        Symbol symbol)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (symbol == Symbol.Empty)
        {
            throw new ArgumentException(
                "A estratégia deve analisar uma jogada para X ou O.",
                nameof(symbol));
        }

        SearchBoard search_board = new(board);
        IReadOnlyList<BoardPosition> available_positions =
            search_board.get_available_positions();

        if (available_positions.Count == 0)
        {
            throw new InvalidOperationException(
                "Não existem casas disponíveis para análise.");
        }

        SearchMetrics metrics = new();
        Symbol opponent_symbol = get_opponent_symbol(symbol);
        BoardPosition best_position = available_positions[0];
        int best_score = int.MinValue;

        foreach (BoardPosition position in available_positions)
        {
            search_board.set_symbol(position, symbol);

            int score = minimax(
                search_board,
                symbol,
                opponent_symbol,
                opponent_symbol,
                depth: 1,
                metrics);

            search_board.set_symbol(position, Symbol.Empty);

            if (score > best_score)
            {
                best_score = score;
                best_position = position;
            }
        }

        return new MinimaxAnalysis(
            best_position,
            best_score,
            metrics.MaximumDepth,
            metrics.VisitedStates);
    }

    private static int minimax(
        SearchBoard board,
        Symbol maximizing_symbol,
        Symbol minimizing_symbol,
        Symbol current_symbol,
        int depth,
        SearchMetrics metrics)
    {
        metrics.register_state(depth);

        if (board.has_winner(maximizing_symbol))
        {
            return WinScore - depth;
        }

        if (board.has_winner(minimizing_symbol))
        {
            return LossScore + depth;
        }

        IReadOnlyList<BoardPosition> available_positions =
            board.get_available_positions();

        if (available_positions.Count == 0)
        {
            return DrawScore;
        }

        bool is_maximizing = current_symbol == maximizing_symbol;
        int best_score = is_maximizing
            ? int.MinValue
            : int.MaxValue;

        foreach (BoardPosition position in available_positions)
        {
            board.set_symbol(position, current_symbol);

            int score = minimax(
                board,
                maximizing_symbol,
                minimizing_symbol,
                get_opponent_symbol(current_symbol),
                depth + 1,
                metrics);

            board.set_symbol(position, Symbol.Empty);

            best_score = is_maximizing
                ? Math.Max(best_score, score)
                : Math.Min(best_score, score);
        }

        return best_score;
    }

    private static Symbol get_opponent_symbol(Symbol symbol)
    {
        return symbol == Symbol.X
            ? Symbol.O
            : Symbol.X;
    }

    /// <summary>
    /// Acumula métricas durante uma única análise.
    /// </summary>
    private sealed class SearchMetrics
    {
        public int MaximumDepth { get; private set; }

        public int VisitedStates { get; private set; }

        public void register_state(int depth)
        {
            VisitedStates++;
            MaximumDepth = Math.Max(MaximumDepth, depth);
        }
    }
}
