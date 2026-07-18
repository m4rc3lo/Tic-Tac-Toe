using TicTacToe.Persistence;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public class JsonSettingsRepositoryTests
{
    [Fact]
    public void load_should_create_directory_and_defaults_when_file_is_missing()
    {
        using TemporaryDirectory directory = new();
        string path = Path.Combine(
            directory.Path,
            "nested",
            "settings.json");
        JsonSettingsRepository repository = create_repository(path);

        ApplicationSettings settings = repository.load();

        Assert.True(File.Exists(path));
        Assert.True(settings.UseUnicode);
        Assert.Equal("Minimax", settings.DefaultStrategy);
    }

    [Fact]
    public void load_should_recover_from_invalid_json()
    {
        using TemporaryDirectory directory = new();
        string path = Path.Combine(directory.Path, "settings.json");
        File.WriteAllText(path, "{ invalid json }");
        JsonSettingsRepository repository = create_repository(path);

        ApplicationSettings settings = repository.load();

        Assert.Equal(40, settings.AnimationDelayMilliseconds);
        Assert.Contains("defaultStrategy", File.ReadAllText(path));
    }

    [Fact]
    public void load_should_ignore_unknown_properties()
    {
        using TemporaryDirectory directory = new();
        string path = Path.Combine(directory.Path, "settings.json");
        File.WriteAllText(
            path,
            """
            {
              "useUnicode": false,
              "unknownFutureProperty": "preserved compatibility",
              "defaultStrategy": "Heuristic",
              "directories": {
                "data": "data",
                "exports": "exports"
              }
            }
            """);
        JsonSettingsRepository repository = create_repository(path);

        ApplicationSettings settings = repository.load();

        Assert.False(settings.UseUnicode);
        Assert.Equal("Heuristic", settings.DefaultStrategy);
    }

    [Fact]
    public void save_should_replace_file_and_remove_temporary_file()
    {
        using TemporaryDirectory directory = new();
        string path = Path.Combine(directory.Path, "settings.json");
        JsonSettingsRepository repository = create_repository(path);
        ApplicationSettings settings =
            ApplicationSettings.create_default();
        settings.RandomSeed = 2026;

        repository.save(settings);
        settings.RandomSeed = 42;
        repository.save(settings);

        string json = File.ReadAllText(path);
        Assert.Contains("42", json);
        Assert.Empty(Directory.GetFiles(
            directory.Path,
            "*.tmp-*",
            SearchOption.AllDirectories));
    }

    [Fact]
    public void load_should_recover_from_recognized_but_invalid_values()
    {
        using TemporaryDirectory directory = new();
        string path = Path.Combine(directory.Path, "settings.json");
        File.WriteAllText(
            path,
            """
            {
              "animationDelayMilliseconds": -10,
              "defaultStrategy": "Unknown",
              "directories": {
                "data": "data",
                "exports": "exports"
              }
            }
            """);
        JsonSettingsRepository repository = create_repository(path);

        ApplicationSettings settings = repository.load();

        Assert.Equal(40, settings.AnimationDelayMilliseconds);
        Assert.Equal("Minimax", settings.DefaultStrategy);
    }

    private static JsonSettingsRepository create_repository(string path)
    {
        return new JsonSettingsRepository(
            path,
            new SettingsValidator());
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        public TemporaryDirectory()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "tic-tac-toe-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
