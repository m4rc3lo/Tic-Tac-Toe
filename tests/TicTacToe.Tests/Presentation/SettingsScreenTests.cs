using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public class SettingsScreenTests
{
    [Fact]
    public void show_should_toggle_all_visual_preferences()
    {
        PresentationPreferences preferences = new(
            use_ansi_colors: false,
            use_unicode: true,
            clear_screen: false,
            visual_effects: true);
        ScreenContext context = new(preferences);
        StringReader reader = new(
            string.Join(
                Environment.NewLine,
                ["1", "2", "3", "4", "0", string.Empty]));
        SettingsScreen screen = new(
            reader,
            new StringWriter());

        ScreenTransition transition = screen.show(context);

        Assert.Equal(ScreenState.MainMenu, transition.Target);
        Assert.False(preferences.UseUnicode);
        Assert.True(preferences.UseAnsiColors);
        Assert.True(preferences.ClearScreen);
        Assert.False(preferences.VisualEffects);
    }
}
