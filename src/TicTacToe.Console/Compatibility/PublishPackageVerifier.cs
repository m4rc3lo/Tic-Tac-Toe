namespace TicTacToe.Compatibility;

/// <summary>
/// Valida arquivos obrigatórios e ausência de dados locais em uma publicação.
/// </summary>
public sealed class PublishPackageVerifier
{
    private static readonly string[] required_files =
    [
        "CITATION.cff",
        "README-PUBLISH.md",
        "settings.example.json"
    ];

    private static readonly string[] forbidden_directories =
    [
        "data",
        "exports"
    ];

    private static readonly string[] forbidden_file_names =
    [
        "settings.json",
        "matches.json",
        "statistics.json",
        "experiment-result.json",
        "experiment-metrics.csv"
    ];

    public PublishPackageValidationResult validate(
        string publish_directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(publish_directory);

        string full_path = Path.GetFullPath(publish_directory);
        List<string> missing = required_files
            .Where(file => !File.Exists(Path.Combine(full_path, file)))
            .ToList();
        List<string> forbidden = [];

        foreach (string directory in forbidden_directories)
        {
            if (Directory.Exists(Path.Combine(full_path, directory)))
            {
                forbidden.Add(directory + Path.DirectorySeparatorChar);
            }
        }

        if (Directory.Exists(full_path))
        {
            forbidden.AddRange(
                Directory.EnumerateFiles(
                        full_path,
                        "*",
                        SearchOption.AllDirectories)
                    .Where(path => forbidden_file_names.Contains(
                        Path.GetFileName(path),
                        StringComparer.OrdinalIgnoreCase))
                    .Select(path => Path.GetRelativePath(full_path, path)));
        }

        return new PublishPackageValidationResult(
            missing.AsReadOnly(),
            forbidden.AsReadOnly());
    }
}
