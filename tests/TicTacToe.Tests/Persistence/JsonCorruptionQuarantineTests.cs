
using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public sealed class JsonCorruptionQuarantineTests : IDisposable
{
    private readonly string directory = Path.Combine(
        Path.GetTempPath(),
        $"tictactoe-quarantine-{Guid.NewGuid():N}");

    [Fact]
    public void settings_should_preserve_invalid_file_before_recovery()
    {
        Directory.CreateDirectory(directory);
        string path = Path.Combine(directory, "settings.json");
        File.WriteAllText(path, "{ invalid");

        ApplicationSettings settings =
            new JsonSettingsRepository(
                path,
                new SettingsValidator())
            .load();

        Assert.True(settings.UseUnicode);
        Assert.True(File.Exists(path));
        Assert.Single(
            Directory.GetFiles(
                directory,
                "settings.json.corrupt-*"));
    }

    [Fact]
    public void history_should_quarantine_invalid_json()
    {
        Directory.CreateDirectory(directory);
        string path = Path.Combine(directory, "matches.json");
        File.WriteAllText(path, "{ invalid");

        IReadOnlyList<MatchRecord> records =
            new JsonMatchHistoryRepository(path).load_all();

        Assert.Empty(records);
        Assert.False(File.Exists(path));
        Assert.Single(
            Directory.GetFiles(
                directory,
                "matches.json.corrupt-*"));
    }


    [Fact]
    public void statistics_should_quarantine_invalid_json()
    {
        Directory.CreateDirectory(directory);
        string path = Path.Combine(directory, "statistics.json");
        File.WriteAllText(path, "{ invalid");

        MatchStatisticsRecord statistics =
            new JsonMatchStatisticsRepository(path).load();

        Assert.Equal(0, statistics.TotalMatches);
        Assert.False(File.Exists(path));
        Assert.Single(
            Directory.GetFiles(
                directory,
                "statistics.json.corrupt-*"));
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
