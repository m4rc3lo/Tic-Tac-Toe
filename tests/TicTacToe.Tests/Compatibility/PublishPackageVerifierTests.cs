using TicTacToe.Compatibility;
using Xunit;

namespace TicTacToe.Tests.Compatibility;

public sealed class PublishPackageVerifierTests : IDisposable
{
    private readonly string directory = Path.Combine(
        Path.GetTempPath(),
        $"tictactoe-publish-{Guid.NewGuid():N}");

    [Fact]
    public void validate_should_accept_clean_package()
    {
        create_required_files();

        PublishPackageValidationResult result =
            new PublishPackageVerifier().validate(directory);

        Assert.True(result.IsValid);
        Assert.Empty(result.MissingFiles);
        Assert.Empty(result.ForbiddenEntries);
    }

    [Fact]
    public void validate_should_report_missing_required_files()
    {
        Directory.CreateDirectory(directory);

        PublishPackageValidationResult result =
            new PublishPackageVerifier().validate(directory);

        Assert.False(result.IsValid);
        Assert.Contains("CITATION.cff", result.MissingFiles);
        Assert.Contains("README-PUBLISH.md", result.MissingFiles);
        Assert.Contains("settings.example.json", result.MissingFiles);
    }

    [Fact]
    public void validate_should_reject_runtime_data()
    {
        create_required_files();
        string data_directory = Path.Combine(directory, "data");
        Directory.CreateDirectory(data_directory);
        File.WriteAllText(
            Path.Combine(data_directory, "matches.json"),
            "[]");

        PublishPackageValidationResult result =
            new PublishPackageVerifier().validate(directory);

        Assert.False(result.IsValid);
        Assert.Contains(
            "data" + Path.DirectorySeparatorChar,
            result.ForbiddenEntries);
        Assert.Contains(
            Path.Combine("data", "matches.json"),
            result.ForbiddenEntries);
    }

    private void create_required_files()
    {
        Directory.CreateDirectory(directory);

        foreach (string file in new[]
                 {
                     "CITATION.cff",
                     "README-PUBLISH.md",
                     "settings.example.json"
                 })
        {
            File.WriteAllText(Path.Combine(directory, file), "content");
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
