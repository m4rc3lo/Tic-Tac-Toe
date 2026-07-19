using TicTacToe.Application;

namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Persiste métricas experimentais acumuladas em CSV.
/// </summary>
public sealed class CsvExperimentResultRepository
    : IExperimentResultRepository
{
    private readonly ExperimentMetricsCsvExporter exporter;

    public CsvExperimentResultRepository(CsvWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        exporter = new ExperimentMetricsCsvExporter(writer);
    }

    public void save(string output_directory, ExperimentResult result)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(output_directory);
        ArgumentNullException.ThrowIfNull(result);
        Directory.CreateDirectory(output_directory);

        exporter.export(
            Path.Combine(output_directory, "experiment-metrics.csv"),
            result.Metrics);
    }
}
