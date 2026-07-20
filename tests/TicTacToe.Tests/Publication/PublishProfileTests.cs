using System.Xml.Linq;
using Xunit;

namespace TicTacToe.Tests.Publication;

public class PublishProfileTests
{
    private static readonly string project_root =
        find_project_root();

    public static IEnumerable<object[]> profiles()
    {
        yield return
        [
            "win-x64-framework-dependent.pubxml",
            "win-x64",
            "false"
        ];
        yield return
        [
            "win-x64-self-contained.pubxml",
            "win-x64",
            "true"
        ];
        yield return
        [
            "linux-x64-framework-dependent.pubxml",
            "linux-x64",
            "false"
        ];
        yield return
        [
            "linux-x64-self-contained.pubxml",
            "linux-x64",
            "true"
        ];
    }

    [Theory]
    [MemberData(nameof(profiles))]
    public void profile_should_define_expected_runtime_and_safe_options(
        string file_name,
        string runtime_identifier,
        string self_contained)
    {
        string path = Path.Combine(
            project_root,
            "src",
            "TicTacToe.Console",
            "Properties",
            "PublishProfiles",
            file_name);
        XDocument document = XDocument.Load(path);

        Assert.Equal(
            runtime_identifier,
            get_property(document, "RuntimeIdentifier"));
        Assert.Equal(
            self_contained,
            get_property(document, "SelfContained"));
        Assert.Equal(
            "false",
            get_property(document, "PublishSingleFile"));
        Assert.Equal(
            "false",
            get_property(document, "PublishTrimmed"));
        Assert.Equal(
            "false",
            get_property(document, "PublishReadyToRun"));
        Assert.Equal(
            "Release",
            get_property(document, "Configuration"));
    }

    [Fact]
    public void project_should_publish_required_distribution_files()
    {
        string path = Path.Combine(
            project_root,
            "src",
            "TicTacToe.Console",
            "TicTacToe.Console.csproj");
        XDocument document = XDocument.Load(path);
        string xml = document.ToString();

        Assert.Contains("CITATION.cff", xml);
        Assert.Contains("README-PUBLISH.md", xml);
        Assert.Contains("settings.example.json", xml);
        Assert.Contains("CopyToPublishDirectory", xml);
    }

    private static string get_property(
        XDocument document,
        string name)
    {
        return document
            .Descendants(name)
            .Single()
            .Value;
    }

    private static string find_project_root()
    {
        DirectoryInfo? current = new(
            AppContext.BaseDirectory);

        while (current is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        current.FullName,
                        "TicTacToe.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException(
            "A raiz do projeto não foi encontrada.");
    }
}
