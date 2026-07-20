
using TicTacToe.Audio;
using TicTacToe.Compatibility;
using TicTacToe.Persistence;
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Composition;

internal static class PresentationComposition
{
    public static PresentationComponents create(
        TextReader reader,
        TextWriter writer,
        RuntimePlatform platform,
        ConsoleCapabilities capabilities,
        PresentationPreferences preferences,
        IMatchPersistenceService persistence_service)
    {
        ConsoleTheme theme = new(preferences);
        AsciiArtCatalog art_catalog = new();
        IDelayService delay_service = new ThreadDelayService();
        IAnimationService animation_service =
            new AnimationService(
                writer,
                delay_service,
                preferences);
        ConsoleBoardRenderer board_renderer =
            new(writer, theme);
        ConsoleGameInput game_input =
            new(reader, writer);
        IVisualFeedbackService visual_feedback =
            new VisualFeedbackService(board_renderer);
        IAudioService enabled_audio =
            new AudioServiceSelector(writer).select(
                audio_enabled: true,
                platform,
                capabilities);
        IAudioService audio_service =
            new PreferenceAwareAudioService(
                preferences,
                enabled_audio);
        ConsoleGameOutput game_output = new(
            writer,
            board_renderer,
            theme,
            art_catalog,
            visual_feedback,
            audio_service);
        IExternalFailureReporter failure_reporter =
            new TextExternalFailureReporter(writer);
        IMoveStrategyFactory strategy_factory =
            new MoveStrategyFactory();
        IMatchSessionRunner match_runner =
            new ConsoleMatchSessionRunner(
                game_input,
                game_output,
                animation_service,
                preferences,
                persistence_service,
                strategy_factory,
                failure_reporter);
        IAutomaticModeControl mode_control =
            new ConsoleAutomaticModeControl(
                writer,
                delay_service,
                () => global::System.Console.KeyAvailable,
                () => global::System.Console.ReadKey(intercept: true),
                capabilities.SupportsInteractiveInput);
        IAutomaticMatchRunner automatic_runner =
            new AutomaticMatchRunner(
                writer,
                game_output,
                delay_service,
                preferences,
                strategy_factory,
                mode_control,
                persistence_service,
                failure_reporter);

        return new PresentationComponents(
            theme,
            art_catalog,
            animation_service,
            match_runner,
            automatic_runner,
            failure_reporter);
    }
}

internal sealed record PresentationComponents(
    ConsoleTheme Theme,
    AsciiArtCatalog ArtCatalog,
    IAnimationService AnimationService,
    IMatchSessionRunner MatchRunner,
    IAutomaticMatchRunner AutomaticRunner,
    IExternalFailureReporter FailureReporter);
