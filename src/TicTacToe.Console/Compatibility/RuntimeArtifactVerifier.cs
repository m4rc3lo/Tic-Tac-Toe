
namespace TicTacToe.Compatibility;

/// <summary>
/// Verifica arquivos obrigatórios no diretório de execução publicado.
/// </summary>
public sealed class RuntimeArtifactVerifier
{
    public IReadOnlyList<string> find_missing(
        string base_directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(base_directory);

        List<string> missing = [];

        if (!File.Exists(
                Path.Combine(base_directory, "CITATION.cff")))
        {
            missing.Add("CITATION.cff");
        }

        return missing.AsReadOnly();
    }
}
