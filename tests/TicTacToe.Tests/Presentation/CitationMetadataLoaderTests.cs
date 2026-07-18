using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public sealed class CitationMetadataLoaderTests : IDisposable
{
    private readonly string directory =
        Path.Combine(
            Path.GetTempPath(),
            $"tictactoe-citation-tests-{Guid.NewGuid():N}");

    [Fact]
    public void load_should_use_current_assembly_version_when_file_is_missing()
    {
        string missing_path =
            Path.Combine(directory, "CITATION.cff");

        CitationMetadata metadata =
            new CitationMetadataLoader().load(missing_path);

        Version? version =
            typeof(CitationMetadataLoader).Assembly
                .GetName()
                .Version;
        string expected = version is null
            ? "unknown"
            : $"{version.Major}.{version.Minor}.{version.Build}";

        Assert.Equal(expected, metadata.Version);
    }

    public void Dispose()
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
