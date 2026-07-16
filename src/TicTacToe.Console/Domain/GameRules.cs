namespace TicTacToe.Domain;

/// <summary>
/// Avalia as regras do jogo da velha sem modificar o tabuleiro recebido.
/// </summary>
public static class GameRules
{
    private static readonly BoardPosition[][] winning_sequences =
    [
        [new BoardPosition(0, 0), new BoardPosition(0, 1), new BoardPosition(0, 2)],
        [new BoardPosition(1, 0), new BoardPosition(1, 1), new BoardPosition(1, 2)],
        [new BoardPosition(2, 0), new BoardPosition(2, 1), new BoardPosition(2, 2)],
        [new BoardPosition(0, 0), new BoardPosition(1, 0), new BoardPosition(2, 0)],
        [new BoardPosition(0, 1), new BoardPosition(1, 1), new BoardPosition(2, 1)],
        [new BoardPosition(0, 2), new BoardPosition(1, 2), new BoardPosition(2, 2)],
        [new BoardPosition(0, 0), new BoardPosition(1, 1), new BoardPosition(2, 2)],
        [new BoardPosition(0, 2), new BoardPosition(1, 1), new BoardPosition(2, 0)]
    ];

    /// <summary>
    /// Avalia o tabuleiro e identifica vitória, empate ou partida em andamento.
    /// </summary>
    /// <param name="board">Tabuleiro a ser avaliado.</param>
    /// <returns>
    /// Resultado consolidado e, em caso de vitória, a sequência vencedora.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Lançada quando o tabuleiro informado é nulo.
    /// </exception>
    public static GameEvaluation evaluate(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        foreach (BoardPosition[] sequence in winning_sequences)
        {
            Symbol first_symbol = board.get_symbol(sequence[0]);

            if (first_symbol == Symbol.Empty)
            {
                continue;
            }

            bool all_equal = sequence
                .Skip(1)
                .All(position => board.get_symbol(position) == first_symbol);

            if (all_equal)
            {
                GameResult result = first_symbol == Symbol.X
                    ? GameResult.XWins
                    : GameResult.OWins;

                return new GameEvaluation(result, sequence);
            }
        }

        return board.IsFull
            ? new GameEvaluation(GameResult.Draw)
            : new GameEvaluation(GameResult.None);
    }
}
