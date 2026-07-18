namespace TicTacToe.Persistence;

/// <summary>
/// Consolida estatísticas de todas as partidas persistidas.
/// </summary>
public sealed record MatchStatisticsRecord(
    int TotalMatches,
    int XWins,
    int OWins,
    int Draws,
    int TotalMoves,
    double AverageMoves,
    double AverageDurationMilliseconds,
    IReadOnlyList<StrategyStatisticsRecord> Strategies)
{
    /// <summary>
    /// Cria estatísticas vazias.
    /// </summary>
    public static MatchStatisticsRecord empty()
    {
        return new MatchStatisticsRecord(
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            Array.Empty<StrategyStatisticsRecord>());
    }
}
