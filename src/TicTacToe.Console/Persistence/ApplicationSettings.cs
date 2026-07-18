namespace TicTacToe.Persistence;

/// <summary>
/// Representa as configurações persistentes da aplicação.
/// </summary>
public sealed class ApplicationSettings
{
    /// <summary>
    /// Obtém ou define se caracteres Unicode devem ser utilizados.
    /// </summary>
    public bool UseUnicode { get; set; } = true;

    /// <summary>
    /// Obtém ou define se sequências de cores ANSI devem ser utilizadas.
    /// </summary>
    public bool UseAnsiColors { get; set; }

    /// <summary>
    /// Obtém ou define se animações e efeitos visuais estão habilitados.
    /// </summary>
    public bool AnimationsEnabled { get; set; } = true;

    /// <summary>
    /// Obtém ou define se sinais sonoros estão habilitados.
    /// </summary>
    public bool AudioEnabled { get; set; } = true;

    /// <summary>
    /// Obtém ou define o atraso-base das animações em milissegundos.
    /// </summary>
    public int AnimationDelayMilliseconds { get; set; } = 40;

    /// <summary>
    /// Obtém ou define se o terminal pode ser limpo entre telas.
    /// </summary>
    public bool ClearScreen { get; set; }

    /// <summary>
    /// Obtém ou define os diretórios utilizados pela aplicação.
    /// </summary>
    public ApplicationDirectories Directories { get; set; } = new();

    /// <summary>
    /// Obtém ou define a Strategy padrão: Random, Heuristic ou Minimax.
    /// </summary>
    public string DefaultStrategy { get; set; } = "Minimax";

    /// <summary>
    /// Obtém ou define a semente pseudoaleatória opcional.
    /// </summary>
    public int? RandomSeed { get; set; }

    /// <summary>
    /// Cria uma nova configuração com os valores padrão do projeto.
    /// </summary>
    public static ApplicationSettings create_default()
    {
        return new ApplicationSettings();
    }
}
