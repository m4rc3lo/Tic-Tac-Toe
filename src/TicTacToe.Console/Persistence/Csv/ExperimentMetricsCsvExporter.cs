namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Exporta métricas de execuções experimentais.
/// </summary>
public sealed class ExperimentMetricsCsvExporter
    : ICsvExporter<ExperimentMetricRecord>
{
    private static readonly string[] header =
    [
        "experiment_id",
        "run_number",
        "x_strategy",
        "o_strategy",
        "seed",
        "result",
        "move_count",
        "duration_ms",
        "evaluated_states",
        "failed",
        "failure_message",
        "application_version"
    ];

    private readonly CsvWriter writer;

    public ExperimentMetricsCsvExporter(CsvWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    public void export(
        string path,
        IEnumerable<ExperimentMetricRecord> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        writer.write(
            path,
            header,
            records.Select(map_row));
    }

    private static IReadOnlyList<string?> map_row(
        ExperimentMetricRecord record)
    {
        return
        [
            record.ExperimentId.ToString("D"),
            CsvWriter.format_number(record.RunNumber),
            record.XStrategy,
            record.OStrategy,
            record.Seed?.ToString(
                System.Globalization.CultureInfo.InvariantCulture),
            record.Result,
            CsvWriter.format_number(record.MoveCount),
            CsvWriter.format_number(record.DurationMilliseconds),
            record.EvaluatedStates?.ToString(
                System.Globalization.CultureInfo.InvariantCulture),
            record.Failed ? "true" : "false",
            record.FailureMessage,
            record.ApplicationVersion
        ];
    }
}
