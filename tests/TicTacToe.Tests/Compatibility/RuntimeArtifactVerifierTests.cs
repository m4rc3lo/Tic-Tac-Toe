
using TicTacToe.Compatibility;
using Xunit;

namespace TicTacToe.Tests.Compatibility;

public sealed class RuntimeArtifactVerifierTests : IDisposable
{
    private readonly string directory = Path.Combine(
        Path.GetTempPath(),
        $"tictactoe-artifacts-{Guid.NewGuid():N}");

    [Fact]
    public void find_missing_should_require_citation_file()
    {
        Directory.CreateDirectory(directory);

        IReadOnlyList<string> missing =
            new RuntimeArtifactVerifier()
                .find_missing(directory);

        Assert.Equal(["CITATION.cff"], missing);
    }

    [Fact]
    public void find_missing_should_accept_citation_file()
    {
        Directory.CreateDirectory(directory);
        File.WriteAllText(
            Path.Combine(directory, "CITATION.cff"),
            "cff-version: 1.2.0");

        IReadOnlyList<string> missing =
            new RuntimeArtifactVerifier()
                .find_missing(directory);

        Assert.Empty(missing);
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
