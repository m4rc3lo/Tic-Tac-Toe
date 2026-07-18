using TicTacToe.Persistence;

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
    /// <param name="audio_enabled">Habilita sinais sonoros.</param>
    /// <param name="animation_delay_milliseconds">Atraso-base das animações.</param>
    public PresentationPreferences(
        bool use_ansi_colors = false,
        bool use_unicode = true,
        bool clear_screen = false,
        bool visual_effects = true,
        bool audio_enabled = true,
        int animation_delay_milliseconds = 40)
    {
        UseAnsiColors = use_ansi_colors;
        UseUnicode = use_unicode;
        ClearScreen = clear_screen;
        VisualEffects = visual_effects;
        AudioEnabled = audio_enabled;
        AnimationDelayMilliseconds = animation_delay_milliseconds;
    }

    public bool UseAnsiColors { get; set; }

    public bool UseUnicode { get; set; }

    public bool ClearScreen { get; set; }

    public bool VisualEffects { get; set; }

    /// <summary>
    /// Obtém ou define se sinais sonoros estão habilitados.
    /// </summary>
    public bool AudioEnabled { get; set; }

    /// <summary>
    /// Obtém ou define o atraso-base das animações em milissegundos.
    /// </summary>
    public int AnimationDelayMilliseconds { get; set; }

    /// <summary>
    /// Converte configurações persistidas para preferências de apresentação.
    /// </summary>
    public static PresentationPreferences from_settings(
        ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return new PresentationPreferences(
            use_ansi_colors: settings.UseAnsiColors,
            use_unicode: settings.UseUnicode,
            clear_screen: settings.ClearScreen,
            visual_effects: settings.AnimationsEnabled,
            audio_enabled: settings.AudioEnabled,
            animation_delay_milliseconds:
                settings.AnimationDelayMilliseconds);
    }
}
