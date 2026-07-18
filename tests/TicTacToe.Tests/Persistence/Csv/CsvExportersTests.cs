using System.Text;
using TicTacToe.Persistence;
using TicTacToe.Persistence.Csv;
using Xunit;

namespace TicTacToe.Tests.Persistence.Csv;

public sealed class CsvExportersTests : IDisposable
{
    private readonly string directory =
        Path.Combine(
            Path.GetTempPath(),
            $"tictactoe-csv-tests-{Guid.NewGuid():N}");

    [Fact]
    public void match_exporter_should_write_expected_schema()
    {
        string path = Path.Combine(directory, "matches.csv");
        MatchRecord record = create_match_record();

        new MatchCsvExporter(new CsvWriter())
            .export(path, [record]);

        string[] lines = File.ReadAllLines(path);

        Assert.Equal(
            "match_id;started_at_utc;finished_at_utc;duration_ms;first_name;first_kind;first_symbol;first_strategy;second_name;second_kind;second_symbol;second_strategy;result;move_count;winning_sequence;random_seed;application_version",
            lines[0]);
        Assert.Equal(2, lines.Length);
        Assert.Contains("\"Pessoa; Um\"", lines[1]);
    }

    [Fact]
    public void move_exporter_should_write_one_row_per_move()
    {
        string path = Path.Combine(directory, "moves.csv");

        new MoveCsvExporter(new CsvWriter())
            .export(path, [create_match_record()]);

        string[] lines = File.ReadAllLines(path);

        Assert.Equal(
            "match_id;turn_number;row;column;symbol",
            lines[0]);
        Assert.Equal(3, lines.Length);
    }

    [Fact]
    public void statistics_exporter_should_use_decimal_point()
    {
        string path =
            Path.Combine(directory, "statistics.csv");
        MatchStatisticsRecord statistics = new(
            2,
            1,
            0,
            1,
            15,
            7.5,
            1250.25,
            Array.Empty<StrategyStatisticsRecord>());

        new StatisticsCsvExporter(new CsvWriter())
            .export_summary(path, statistics);

        string content = File.ReadAllText(path);

        Assert.Contains("7.5;1250.25", content);
        Assert.DoesNotContain("7,5", content);
    }

    [Fact]
    public void experiment_exporter_should_escape_failure_message()
    {
        string path =
            Path.Combine(directory, "experiment.csv");
        ExperimentMetricRecord metric = new(
            Guid.NewGuid(),
            1,
            "Minimax",
            "Random",
            42,
            "Failed",
            0,
            10,
            null,
            true,
            "Falha; com \"aspas\"\ne linha",
            "1.8.0");

        new ExperimentMetricsCsvExporter(
            new CsvWriter())
            .export(path, [metric]);

        string content = File.ReadAllText(path);

        Assert.Contains(
            "\"Falha; com \"\"aspas\"\"",
            content);
        Assert.Contains("e linha\"", content);
    }

    [Fact]
    public void writer_should_close_temporary_file_before_replacement()
    {
        string path =
            Path.Combine(directory, "replacement.csv");
        CsvWriter writer = new();

        writer.write(
            path,
            ["value"],
            [["first"]]);

        writer.write(
            path,
            ["value"],
            [["second"]]);

        Assert.Equal(
            string.Join(
                Environment.NewLine,
                ["value", "second", string.Empty]),
            File.ReadAllText(path));
    }

    [Fact]
    public void writer_should_use_utf8_without_bom()
    {
        string path = Path.Combine(directory, "utf8.csv");

        new CsvWriter().write(
            path,
            ["nome"],
            [["José"]]);

        byte[] bytes = File.ReadAllBytes(path);

        Assert.False(
            bytes.Length >= 3 &&
            bytes[0] == 0xEF &&
            bytes[1] == 0xBB &&
            bytes[2] == 0xBF);
        Assert.Contains(
            "José",
            Encoding.UTF8.GetString(bytes));
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static MatchRecord create_match_record()
    {
        DateTimeOffset start =
            new(2026, 7, 17, 12, 0, 0, TimeSpan.Zero);

        return new MatchRecord(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            start,
            start.AddMilliseconds(500),
            500,
            new MatchParticipantRecord(
                "Pessoa; Um",
                "Human",
                "X",
                null),
            new MatchParticipantRecord(
                "CPU",
                "Computer",
                "O",
                "Minimax"),
            [
                new MatchMoveRecord(1, 0, 0, "X"),
                new MatchMoveRecord(2, 1, 1, "O")
            ],
            "Draw",
            Array.Empty<BoardPositionRecord>(),
            42,
            "1.8.0");
    }
}
