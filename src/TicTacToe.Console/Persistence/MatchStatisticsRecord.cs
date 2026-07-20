
namespace TicTacToe.Persistence;

/// <summary>
/// Consolida estatísticas de todas as partidas persistidas.
/// </summary>
public sealed record MatchStatisticsRecord
{
    private IReadOnlyList<StrategyStatisticsRecord> strategies =
        Array.Empty<StrategyStatisticsRecord>();

    public MatchStatisticsRecord()
    {
    }

    public MatchStatisticsRecord(
        int total_matches,
        int x_wins,
        int o_wins,
        int draws,
        int total_moves,
        double average_moves,
        double average_duration_milliseconds,
        IEnumerable<StrategyStatisticsRecord> strategies)
    {
        TotalMatches = total_matches;
        XWins = x_wins;
        OWins = o_wins;
        Draws = draws;
        TotalMoves = total_moves;
        AverageMoves = average_moves;
        AverageDurationMilliseconds = average_duration_milliseconds;
        Strategies = strategies.ToArray();
    }

    public int TotalMatches { get; init; }
    public int XWins { get; init; }
    public int OWins { get; init; }
    public int Draws { get; init; }
    public int TotalMoves { get; init; }
    public double AverageMoves { get; init; }
    public double AverageDurationMilliseconds { get; init; }

    public IReadOnlyList<StrategyStatisticsRecord> Strategies
    {
        get => strategies;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            strategies = Array.AsReadOnly(value.ToArray());
        }
    }

    public static MatchStatisticsRecord empty()
    {
        return new MatchStatisticsRecord();
    }
}
