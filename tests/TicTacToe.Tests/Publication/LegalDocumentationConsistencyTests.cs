using System.Xml.Linq;
using Xunit;

namespace TicTacToe.Tests.Publication;

public class LegalDocumentationConsistencyTests
{
    private static readonly string project_root =
        find_project_root();

    [Fact]
    public void citation_version_should_match_assembly_version()
    {
        XDocument props = XDocument.Load(
            Path.Combine(
                project_root,
                "Directory.Build.props"));
        string assembly_version = props
            .Descendants("Version")
            .Single()
            .Value;
        string citation = File.ReadAllText(
            Path.Combine(
                project_root,
                "CITATION.cff"));

        Assert.Contains(
            $"version: \"{assembly_version}\"",
            citation);
    }


    [Fact]
    public void release_should_use_expected_version_and_date()
    {
        string citation = File.ReadAllText(
            Path.Combine(
                project_root,
                "CITATION.cff"));
        string changelog = File.ReadAllText(
            Path.Combine(
                project_root,
                "CHANGELOG.md"));

        Assert.Contains(
            "version: \"2.0.0\"",
            citation);
        Assert.Contains(
            "date-released: \"2026-07-22\"",
            citation);
        Assert.Contains(
            "## [2.0.0] - 2026-07-22",
            changelog);
    }

    [Fact]
    public void production_project_should_not_reference_nuget_packages()
    {
        XDocument project = XDocument.Load(
            Path.Combine(
                project_root,
                "src",
                "TicTacToe.Console",
                "TicTacToe.Console.csproj"));

        Assert.Empty(project.Descendants("PackageReference"));
    }

    [Fact]
    public void every_document_should_be_listed_in_documentation_index()
    {
        string docs_directory = Path.Combine(
            project_root,
            "docs");
        string index = File.ReadAllText(
            Path.Combine(
                docs_directory,
                "README.md"));

        foreach (string path in Directory.EnumerateFiles(
                     docs_directory,
                     "*.md"))
        {
            string file_name = Path.GetFileName(path);

            if (file_name == "README.md")
            {
                continue;
            }

            Assert.Contains(file_name, index);
        }
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
