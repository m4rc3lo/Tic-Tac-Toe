namespace TicTacToe.Compatibility;

/// <summary>
/// Contém o resultado da inspeção de um diretório publicado.
/// </summary>
public sealed record PublishPackageValidationResult(
    IReadOnlyList<string> MissingFiles,
    IReadOnlyList<string> ForbiddenEntries)
{
    public bool IsValid =>
        MissingFiles.Count == 0 &&
        ForbiddenEntries.Count == 0;
}
