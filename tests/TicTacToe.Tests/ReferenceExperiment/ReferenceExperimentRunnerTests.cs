
using System.Text.Json;
using TicTacToe.ReferenceExperiment;
using Xunit;

namespace TicTacToe.Tests.ReferenceExperiment;

public sealed class ReferenceExperimentRunnerTests : IDisposable
{
    private readonly string directory = Path.Combine(
        Path.GetTempPath(),
        $"tictactoe-reference-runner-{Guid.NewGuid():N}");

    [Fact]
    public void run_should_persist_final_consistent_artifacts()
    {
        ReferenceExperimentPlan plan = new(
            RepetitionsPerScenario: 2,
            BaseSeed: 100,
            AlternateSymbols: true,
            AlternateFirstParticipant: true,
            OutputDirectory: directory);

        ReferenceExperimentManifest manifest =
            new ReferenceExperimentRunner().run(
                plan,
                "test-commit");

        Assert.Equal(6, manifest.Scenarios.Count);

        foreach (ReferenceScenarioArtifact scenario in manifest.Scenarios)
        {
            string scenario_directory = Path.Combine(
                directory,
                scenario.Directory);
            string json_path = Path.Combine(
                scenario_directory,
                scenario.JsonFile);
            string csv_path = Path.Combine(
                scenario_directory,
                scenario.CsvFile);

            using JsonDocument document =
                JsonDocument.Parse(File.ReadAllText(json_path));

            int total_runs = document.RootElement
                .GetProperty("summary")
                .GetProperty("totalRuns")
                .GetInt32();
            int metrics = document.RootElement
                .GetProperty("metrics")
                .GetArrayLength();
            int csv_rows =
                File.ReadLines(csv_path).Count() - 1;

            Assert.Equal(2, total_runs);
            Assert.Equal(total_runs, metrics);
            Assert.Equal(total_runs, csv_rows);
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
