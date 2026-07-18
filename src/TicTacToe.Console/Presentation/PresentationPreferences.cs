namespace TicTacToe.Presentation;

/// <summary>
/// Contém preferências visuais fornecidas à camada de apresentação.
/// </summary>
public sealed class PresentationPreferences
{
    /// <summary>
    /// Inicializa preferências com valores seguros e configuráveis.
    /// </summary>
    /// <param name="use_ansi_colors">Habilita sequências de cor ANSI.</param>
    /// <param name="use_unicode">Habilita caracteres Unicode.</param>
    /// <param name="clear_screen">Habilita limpeza do terminal entre telas.</param>
    /// <param name="visual_effects">Habilita artes e efeitos visuais.</param>
    public PresentationPreferences(
        bool use_ansi_colors = false,
        bool use_unicode = true,
        bool clear_screen = false,
        bool visual_effects = true,
        bool audio_enabled = true)
    {
        UseAnsiColors = use_ansi_colors;
        UseUnicode = use_unicode;
        ClearScreen = clear_screen;
        VisualEffects = visual_effects;
        AudioEnabled = audio_enabled;
    }

    public bool UseAnsiColors { get; set; }

    public bool UseUnicode { get; set; }

    public bool ClearScreen { get; set; }

    public bool VisualEffects { get; set; }

    /// <summary>
    /// Obtém ou define se sinais sonoros estão habilitados.
    /// </summary>
    public bool AudioEnabled { get; set; }
}
