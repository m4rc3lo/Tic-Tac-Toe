namespace TicTacToe.Persistence;

/// <summary>
/// Agrupa os diretórios configuráveis utilizados pela aplicação.
/// </summary>
public sealed class ApplicationDirectories
{
    /// <summary>
    /// Diretório para dados persistidos.
    /// </summary>
    public string Data { get; set; } = "data";

    /// <summary>
    /// Diretório para exportações.
    /// </summary>
    public string Exports { get; set; } = "exports";
}
