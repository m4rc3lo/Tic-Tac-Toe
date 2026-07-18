namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Coordena a exportação dos conjuntos persistidos para arquivos CSV.
/// </summary>
public sealed class CsvExportService
{
    private readonly MatchCsvExporter match_exporter;
    private readonly MoveCsvExporter move_exporter;
    private readonly StatisticsCsvExporter statistics_exporter;
    private readonly ExperimentMetricsCsvExporter experiment_exporter;

    public CsvExportService(CsvWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        match_exporter = new MatchCsvExporter(writer);
        move_exporter = new MoveCsvExporter(writer);
        statistics_exporter =
            new StatisticsCsvExporter(writer);
        experiment_exporter =
            new ExperimentMetricsCsvExporter(writer);
    }

    public void export_matches(
        string directory,
        IReadOnlyList<MatchRecord> matches,
        MatchStatisticsRecord statistics)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentNullException.ThrowIfNull(statistics);

        Directory.CreateDirectory(directory);

        match_exporter.export(
            Path.Combine(directory, "matches.csv"),
            matches);
        move_exporter.export(
            Path.Combine(directory, "moves.csv"),
            matches);
        statistics_exporter.export_summary(
            Path.Combine(directory, "statistics.csv"),
            statistics);
        statistics_exporter.export_strategies(
            Path.Combine(
                directory,
                "strategy-statistics.csv"),
            statistics);
    }

    public void export_experiment_metrics(
        string directory,
        IEnumerable<ExperimentMetricRecord> metrics)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        ArgumentNullException.ThrowIfNull(metrics);

        Directory.CreateDirectory(directory);

        experiment_exporter.export(
            Path.Combine(
                directory,
                "experiment-metrics.csv"),
            metrics);
    }
}
