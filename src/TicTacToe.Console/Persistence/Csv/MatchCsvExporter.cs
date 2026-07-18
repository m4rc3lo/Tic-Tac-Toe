namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Exporta uma linha por partida.
/// </summary>
public sealed class MatchCsvExporter
    : ICsvExporter<MatchRecord>
{
    private static readonly string[] header =
    [
        "match_id",
        "started_at_utc",
        "finished_at_utc",
        "duration_ms",
        "first_name",
        "first_kind",
        "first_symbol",
        "first_strategy",
        "second_name",
        "second_kind",
        "second_symbol",
        "second_strategy",
        "result",
        "move_count",
        "winning_sequence",
        "random_seed",
        "application_version"
    ];

    private readonly CsvWriter writer;

    public MatchCsvExporter(CsvWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    public void export(
        string path,
        IEnumerable<MatchRecord> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        writer.write(
            path,
            header,
            records.Select(map_row));
    }

    private static IReadOnlyList<string?> map_row(
        MatchRecord record)
    {
        string winning_sequence = string.Join(
            "|",
            record.WinningSequence.Select(
                position =>
                    $"{position.Row},{position.Column}"));

        return
        [
            record.Id.ToString("D"),
            CsvWriter.format_date(record.StartedAt),
            CsvWriter.format_date(record.FinishedAt),
            CsvWriter.format_number(record.DurationMilliseconds),
            record.FirstParticipant.Name,
            record.FirstParticipant.Kind,
            record.FirstParticipant.Symbol,
            record.FirstParticipant.Strategy,
            record.SecondParticipant.Name,
            record.SecondParticipant.Kind,
            record.SecondParticipant.Symbol,
            record.SecondParticipant.Strategy,
            record.Result,
            CsvWriter.format_number(record.Moves.Count),
            winning_sequence,
            record.RandomSeed?.ToString(
                System.Globalization.CultureInfo.InvariantCulture),
            record.ApplicationVersion
        ];
    }
}
