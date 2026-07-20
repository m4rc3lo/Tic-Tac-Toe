
using TicTacToe.Compatibility;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Compatibility;

public class CompatibilityModeTests
{
    [Fact]
    public void apply_should_disable_terminal_features_when_output_is_redirected()
    {
        PresentationPreferences requested = new(
            use_ansi_colors: true,
            use_unicode: true,
            clear_screen: true,
            visual_effects: true,
            audio_enabled: true,
            animation_delay_milliseconds: 40);
        ConsoleCapabilities capabilities = new(
            IsInputRedirected: false,
            IsOutputRedirected: true,
            SupportsInteractiveInput: true,
            SupportsUnicode: true,
            SupportsAnsi: false,
            SupportsClearScreen: false,
            SupportsConsoleBeep: false,
            SupportsTerminalBell: false);

        PresentationPreferences effective =
            new CompatibilityMode().apply(
                requested,
                capabilities);

        Assert.False(effective.UseAnsiColors);
        Assert.True(effective.UseUnicode);
        Assert.False(effective.ClearScreen);
        Assert.False(effective.VisualEffects);
        Assert.False(effective.AudioEnabled);
        Assert.Equal(0, effective.AnimationDelayMilliseconds);
    }

    [Fact]
    public void apply_should_preserve_supported_requested_features()
    {
        PresentationPreferences requested = new(
            use_ansi_colors: true,
            use_unicode: true,
            clear_screen: true,
            visual_effects: true,
            audio_enabled: true,
            animation_delay_milliseconds: 25);
        ConsoleCapabilities capabilities = new(
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            false);

        PresentationPreferences effective =
            new CompatibilityMode().apply(
                requested,
                capabilities);

        Assert.True(effective.UseAnsiColors);
        Assert.True(effective.UseUnicode);
        Assert.True(effective.ClearScreen);
        Assert.True(effective.VisualEffects);
        Assert.True(effective.AudioEnabled);
        Assert.Equal(25, effective.AnimationDelayMilliseconds);
    }
}
