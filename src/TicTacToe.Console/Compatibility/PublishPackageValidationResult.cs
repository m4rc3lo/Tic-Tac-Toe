
namespace TicTacToe.Compatibility;

/// <summary>
/// Contém o resultado da inspeção de um diretório publicado.
/// </summary>
public sealed record PublishPackageValidationResult
{
    public PublishPackageValidationResult(
        IEnumerable<string> missing_files,
        IEnumerable<string> forbidden_entries)
    {
        ArgumentNullException.ThrowIfNull(missing_files);
        ArgumentNullException.ThrowIfNull(forbidden_entries);

        MissingFiles = Array.AsReadOnly(
            missing_files.ToArray());
        ForbiddenEntries = Array.AsReadOnly(
            forbidden_entries.ToArray());
    }

    public IReadOnlyList<string> MissingFiles { get; }
    public IReadOnlyList<string> ForbiddenEntries { get; }

    public bool IsValid =>
        MissingFiles.Count == 0 &&
        ForbiddenEntries.Count == 0;
}
