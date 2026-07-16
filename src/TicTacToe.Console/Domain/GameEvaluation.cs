namespace TicTacToe.Domain;

/// <summary>
/// Representa o resultado da avaliação das regras sobre um tabuleiro.
/// </summary>
public sealed class GameEvaluation
{
    private static readonly IReadOnlyList<BoardPosition> empty_positions =
        Array.Empty<BoardPosition>();

    /// <summary>
    /// Inicializa uma nova avaliação de partida.
    /// </summary>
    /// <param name="result">Resultado identificado no tabuleiro.</param>
    /// <param name="winning_positions">
    /// Posições que formam a sequência vencedora, quando houver.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Lançada quando uma vitória não possui exatamente três posições ou quando
    /// um resultado sem vitória recebe posições vencedoras.
    /// </exception>
    public GameEvaluation(
        GameResult result,
        IEnumerable<BoardPosition>? winning_positions = null)
    {
        BoardPosition[] positions = winning_positions?.ToArray()
            ?? Array.Empty<BoardPosition>();

        bool is_win = result is GameResult.XWins or GameResult.OWins;

        if (is_win && positions.Length != BoardPosition.BoardSize)
        {
            throw new ArgumentException(
                "Uma vitória deve possuir exatamente três posições vencedoras.",
                nameof(winning_positions));
        }

        if (!is_win && positions.Length != 0)
        {
            throw new ArgumentException(
                "Resultados sem vitória não podem possuir posições vencedoras.",
                nameof(winning_positions));
        }

        Result = result;
        WinningPositions = positions.Length == 0
            ? empty_positions
            : Array.AsReadOnly(positions);
    }

    /// <summary>
    /// Obtém o resultado identificado.
    /// </summary>
    public GameResult Result { get; }

    /// <summary>
    /// Obtém as posições que formam a sequência vencedora.
    /// </summary>
    public IReadOnlyList<BoardPosition> WinningPositions { get; }

    /// <summary>
    /// Obtém um valor que indica se a partida ainda está em andamento.
    /// </summary>
    public bool IsInProgress => Result == GameResult.None;

    /// <summary>
    /// Obtém um valor que indica se a avaliação representa uma vitória.
    /// </summary>
    public bool HasWinner => Result is GameResult.XWins or GameResult.OWins;
}
