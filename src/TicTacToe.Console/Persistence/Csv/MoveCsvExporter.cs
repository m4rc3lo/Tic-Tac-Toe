namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Exporta uma linha por jogada.
/// </summary>
public sealed class MoveCsvExporter
{
    private static readonly string[] header =
    [
        "match_id",
        "turn_number",
        "row",
        "column",
        "symbol"
    ];

    private readonly CsvWriter writer;

    public MoveCsvExporter(CsvWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    public void export(
        string path,
        IEnumerable<MatchRecord> matches)
    {
        ArgumentNullException.ThrowIfNull(matches);

        IEnumerable<IReadOnlyList<string?>> rows =
            matches.SelectMany(
                match => match.Moves.Select(
                    move => map_row(match.Id, move)));

        writer.write(path, header, rows);
    }

    private static IReadOnlyList<string?> map_row(
        Guid match_id,
        MatchMoveRecord move)
    {
        return
        [
            match_id.ToString("D"),
            CsvWriter.format_number(move.TurnNumber),
            CsvWriter.format_number(move.Row),
            CsvWriter.format_number(move.Column),
            move.Symbol
        ];
    }
}
