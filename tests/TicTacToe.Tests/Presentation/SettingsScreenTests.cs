using TicTacToe.Persistence;
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public class SettingsScreenTests
{
    [Fact]
    public void show_should_toggle_and_persist_preferences()
    {
        ApplicationSettings settings = new();
        PresentationPreferences preferences =
            PresentationPreferences.from_settings(settings);
        RecordingSettingsRepository repository = new();
        ScreenContext context = new(
            preferences,
            settings,
            repository);
        StringReader reader = new(
            string.Join(
                Environment.NewLine,
                ["1", "2", "3", "4", "5", "6", "125", "0", string.Empty]));
        SettingsScreen screen = new(reader, new StringWriter());

        ScreenTransition transition = screen.show(context);

        Assert.Equal(ScreenState.MainMenu, transition.Target);
        Assert.False(preferences.UseUnicode);
        Assert.True(preferences.UseAnsiColors);
        Assert.True(preferences.ClearScreen);
        Assert.False(preferences.VisualEffects);
        Assert.False(preferences.AudioEnabled);
        Assert.Equal(125, preferences.AnimationDelayMilliseconds);
        Assert.Equal(1, repository.SaveCount);
        Assert.False(repository.Saved!.UseUnicode);
        Assert.Equal(125, repository.Saved.AnimationDelayMilliseconds);
    }

    private sealed class RecordingSettingsRepository
        : ISettingsRepository
    {
        public int SaveCount { get; private set; }
        public ApplicationSettings? Saved { get; private set; }
        public ApplicationSettings load() =>
            ApplicationSettings.create_default();
        public void save(ApplicationSettings settings)
        {
            SaveCount++;
            Saved = settings;
        }
    }
}
