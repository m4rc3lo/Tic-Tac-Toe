
using TicTacToe.Compatibility;
using TicTacToe.Persistence;
using TicTacToe.Presentation;

namespace TicTacToe.Composition;

internal static class CompatibilityComposition
{
    public static CompatibilityComponents create(
        ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        RuntimePlatform platform =
            new SystemPlatformDetector().detect();
        ConsoleCapabilities capabilities =
            new SystemConsoleCapabilityDetector()
                .detect(platform);
        PresentationPreferences effective =
            new CompatibilityMode().apply(
                PresentationPreferences.from_settings(settings),
                capabilities);

        return new CompatibilityComponents(
            platform,
            capabilities,
            effective);
    }
}

internal sealed record CompatibilityComponents(
    RuntimePlatform Platform,
    ConsoleCapabilities Capabilities,
    PresentationPreferences Preferences);
