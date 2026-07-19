using TicTacToe.Application;
using TicTacToe.Persistence;
using TicTacToe.Persistence.Csv;
using Xunit;

namespace TicTacToe.Tests.Persistence;

public sealed class ExperimentResultRepositoriesTests : IDisposable
{
    private readonly string directory = Path.Combine(
        Path.GetTempPath(),
        $"tictactoe-experiment-{Guid.NewGuid():N}");

    [Fact]
    public void repositories_should_write_json_and_csv_atomically()
    {
        ExperimentResult result = create_result();

        new JsonExperimentResultRepository().save(directory, result);
        new CsvExperimentResultRepository(new CsvWriter()).save(directory, result);

        Assert.True(File.Exists(Path.Combine(directory, "experiment-result.json")));
        Assert.True(File.Exists(Path.Combine(directory, "experiment-metrics.csv")));
        Assert.Contains("experimentId", File.ReadAllText(Path.Combine(directory, "experiment-result.json")));
        Assert.Contains("experiment_id;run_number", File.ReadAllText(Path.Combine(directory, "experiment-metrics.csv")));
        Assert.Empty(Directory.GetFiles(directory, "*.tmp-*", SearchOption.AllDirectories));
    }

    public void Dispose()
    {
        if (Directory.Exists(directory)) Directory.Delete(directory, true);
    }

    private static ExperimentResult create_result()
    {
        Guid id = Guid.NewGuid();
        ExperimentConfiguration configuration = new(
            ExperimentStrategy.Random,
            ExperimentStrategy.Minimax,
            1,
            false,
            false,
            1,
            "1.8.0",
            OutputDirectory: ".");
        ExperimentMetricRecord metric = new(
            id, 1, "Random", "Minimax", 1, "Draw", 9, 10, 20,
            false, null, "1.8.0");
        return new ExperimentResult(
            id,
            configuration,
            [metric],
            new ExperimentSummary(1, 1, 0, 0, 0, 1, 9, 10));
    }
}
