using TicTacToe.ReferenceExperiment;
using Xunit;

namespace TicTacToe.Tests.ReferenceExperiment;

public sealed class ReferenceExperimentCommandTests : IDisposable
{
    private readonly string directory = Path.Combine(
        Path.GetTempPath(),
        $"tictactoe-reference-command-{Guid.NewGuid():N}");

    [Fact]
    public void try_run_should_accept_short_reproducible_plan()
    {
        StringWriter writer = new();

        bool handled = ReferenceExperimentCommand.try_run(
        [
            "--reference-experiment",
            "--commit",
            "test-commit",
            "--output",
            directory,
            "--repetitions",
            "2",
            "--base-seed",
            "1900"
        ],
        writer);

        Assert.True(handled);
        Assert.Contains(
            "Repetições por cenário: 2.",
            writer.ToString());
        Assert.Contains(
            "Semente base: 1900.",
            writer.ToString());
        Assert.True(
            File.Exists(
                Path.Combine(
                    directory,
                    "reference-manifest.json")));
    }

    [Fact]
    public void try_run_should_reject_zero_repetitions()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => ReferenceExperimentCommand.try_run(
            [
                "--reference-experiment",
                "--repetitions",
                "0"
            ],
            new StringWriter()));
    }

    [Fact]
    public void try_run_should_ignore_unrelated_arguments()
    {
        bool handled =
            ReferenceExperimentCommand.try_run(
            ["--help"],
            new StringWriter());

        Assert.False(handled);
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
}
