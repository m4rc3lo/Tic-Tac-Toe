
using TicTacToe.Persistence;

namespace TicTacToe.Composition;

internal static class SettingsComposition
{
    public static SettingsComponents create()
    {
        string settings_path = Path.Combine(
            AppContext.BaseDirectory,
            "data",
            "settings.json");
        ISettingsRepository repository =
            new JsonSettingsRepository(
                settings_path,
                new SettingsValidator());

        return new SettingsComponents(
            repository.load(),
            repository);
    }
}

internal sealed record SettingsComponents(
    ApplicationSettings Settings,
    ISettingsRepository Repository);
