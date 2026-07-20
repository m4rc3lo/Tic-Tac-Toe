
using TicTacToe.Presentation;

namespace TicTacToe.Compatibility;

/// <summary>
/// Adapta preferências efetivas às capacidades do ambiente atual.
/// </summary>
public sealed class CompatibilityMode
{
    public PresentationPreferences apply(
        PresentationPreferences requested,
        ConsoleCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(requested);
        ArgumentNullException.ThrowIfNull(capabilities);

        bool visual_effects =
            requested.VisualEffects &&
            !capabilities.IsOutputRedirected;

        return new PresentationPreferences(
            use_ansi_colors:
                requested.UseAnsiColors &&
                capabilities.SupportsAnsi,
            use_unicode:
                requested.UseUnicode &&
                capabilities.SupportsUnicode,
            clear_screen:
                requested.ClearScreen &&
                capabilities.SupportsClearScreen,
            visual_effects: visual_effects,
            audio_enabled:
                requested.AudioEnabled &&
                (capabilities.SupportsConsoleBeep ||
                 capabilities.SupportsTerminalBell),
            animation_delay_milliseconds:
                visual_effects
                    ? requested.AnimationDelayMilliseconds
                    : 0);
    }
}
