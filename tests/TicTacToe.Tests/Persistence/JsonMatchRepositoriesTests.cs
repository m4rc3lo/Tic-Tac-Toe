using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public sealed class JsonMatchRepositoriesTests : IDisposable
{
    private readonly string directory =
        Path.Combine(
            Path.GetTempPath(),
            $"tictactoe-match-tests-{Guid.NewGuid():N}");

    [Fact]
    public void history_repository_should_create_directory_and_round_trip()
    {
        string path =
            Path.Combine(directory, "nested", "matches.json");
        JsonMatchHistoryRepository repository = new(path);
        MatchRecord record = create_record();

        repository.replace_all([record]);
        IReadOnlyList<MatchRecord> loaded =
            repository.load_all();

        Assert.Single(loaded);
        Assert.Equal(record.Id, loaded[0].Id);
        Assert.True(File.Exists(path));
        Assert.Empty(
            Directory.GetFiles(
                directory,
                "*.tmp-*",
                SearchOption.AllDirectories));
    }

    [Fact]
    public void statistics_repository_should_round_trip()
    {
        string path =
            Path.Combine(directory, "statistics.json");
        JsonMatchStatisticsRepository repository =
            new(path);
        MatchStatisticsRecord statistics = new(
            2,
            1,
            0,
            1,
            14,
            7,
            150,
            [
                new StrategyStatisticsRecord(
                    "Minimax",
                    2,
                    0,
                    1,
                    1)
            ]);

        repository.save(statistics);
        MatchStatisticsRecord loaded =
            repository.load();

        Assert.Equal(statistics.TotalMatches, loaded.TotalMatches);
        Assert.Equal(statistics.TotalMoves, loaded.TotalMoves);
        Assert.Equal(
            statistics.AverageDurationMilliseconds,
            loaded.AverageDurationMilliseconds);
        Assert.Single(loaded.Strategies);
        Assert.Equal(
            "Minimax",
            loaded.Strategies[0].Strategy);
    }

    [Fact]
    public void repositories_should_recover_from_invalid_json()
    {
        Directory.CreateDirectory(directory);
        string history_path =
            Path.Combine(directory, "matches.json");
        string statistics_path =
            Path.Combine(directory, "statistics.json");

        File.WriteAllText(history_path, "{ invalid");
        File.WriteAllText(statistics_path, "{ invalid");

        Assert.Empty(
            new JsonMatchHistoryRepository(
                history_path).load_all());
        Assert.Equal(
            MatchStatisticsRecord.empty(),
            new JsonMatchStatisticsRepository(
                statistics_path).load());
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(
                directory,
                recursive: true);
        }
    }

    private static MatchRecord create_record()
    {
        return new MatchRecord(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            100,
            new MatchParticipantRecord(
                "Pessoa",
                "Human",
                "X",
                null),
            new MatchParticipantRecord(
                "CPU",
                "Computer",
                "O",
                "Minimax"),
            Array.Empty<MatchMoveRecord>(),
            "Draw",
            Array.Empty<BoardPositionRecord>(),
            null,
            "1.7.0");
    }
}
