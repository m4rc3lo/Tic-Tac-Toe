namespace TicTacToe.Presentation;

/// <summary>
/// Carrega campos simples de crédito a partir de CITATION.cff.
/// </summary>
public sealed class CitationMetadataLoader
{
    public CitationMetadata load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (!File.Exists(path))
        {
            return create_fallback();
        }

        string[] lines = File.ReadAllLines(path);

        string title = get_scalar(lines, "title") ?? "Tic-Tac-Toe Console AI";
        string version = get_scalar(lines, "version") ?? "desconhecida";
        string license = get_scalar(lines, "license") ?? "Apache-2.0";
        string repository =
            get_scalar(lines, "repository-code") ??
            "https://github.com/m4rc3lo/Tic-Tac-Toe";

        string family_names =
            get_scalar(lines, "family-names") ?? "Lopes";
        string given_names =
            get_scalar(lines, "given-names") ?? "Marcelo Dornbusch";

        return new CitationMetadata(
            title,
            version,
            $"{given_names} {family_names}",
            license,
            repository);
    }

    private static string? get_scalar(
        IEnumerable<string> lines,
        string key)
    {
        string prefix = $"{key}:";

        string? line = lines.FirstOrDefault(
            candidate =>
                candidate.TrimStart().StartsWith(
                    prefix,
                    StringComparison.Ordinal));

        if (line is null)
        {
            return null;
        }

        return line.Trim()
            .Substring(prefix.Length)
            .Trim()
            .Trim('"');
    }

    private static CitationMetadata create_fallback()
    {
        return new CitationMetadata(
            "Tic-Tac-Toe Console AI",
            "1.6.0",
            "Marcelo Dornbusch Lopes",
            "Apache-2.0",
            "https://github.com/m4rc3lo/Tic-Tac-Toe");
    }
}
