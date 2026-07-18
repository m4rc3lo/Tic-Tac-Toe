namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Exporta estatísticas gerais e por Strategy.
/// </summary>
public sealed class StatisticsCsvExporter
{
    private static readonly string[] summary_header =
    [
        "total_matches",
        "x_wins",
        "o_wins",
        "draws",
        "total_moves",
        "average_moves",
        "average_duration_ms"
    ];

    private static readonly string[] strategy_header =
    [
        "strategy",
        "matches",
        "wins",
        "losses",
        "draws"
    ];

    private readonly CsvWriter writer;

    public StatisticsCsvExporter(CsvWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    public void export_summary(
        string path,
        MatchStatisticsRecord statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);

        writer.write(
            path,
            summary_header,
            [
                [
                    CsvWriter.format_number(statistics.TotalMatches),
                    CsvWriter.format_number(statistics.XWins),
                    CsvWriter.format_number(statistics.OWins),
                    CsvWriter.format_number(statistics.Draws),
                    CsvWriter.format_number(statistics.TotalMoves),
                    CsvWriter.format_number(statistics.AverageMoves),
                    CsvWriter.format_number(
                        statistics.AverageDurationMilliseconds)
                ]
            ]);
    }

    public void export_strategies(
        string path,
        MatchStatisticsRecord statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);

        writer.write(
            path,
            strategy_header,
            statistics.Strategies.Select(
                strategy =>
                    (IReadOnlyList<string?>)
                    [
                        strategy.Strategy,
                        CsvWriter.format_number(strategy.Matches),
                        CsvWriter.format_number(strategy.Wins),
                        CsvWriter.format_number(strategy.Losses),
                        CsvWriter.format_number(strategy.Draws)
                    ]));
    }
}
