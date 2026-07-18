using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;

namespace TicTacToe;

/// <summary>
/// Define o ponto de composição da aplicação Console.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        TextReader reader = global::System.Console.In;
        TextWriter writer = global::System.Console.Out;

        PresentationPreferences preferences = new();
        ConsoleTheme theme = new(preferences);
        AsciiArtCatalog art_catalog = new();
        IAnimationService animation_service =
            new AnimationService(
                writer,
                new ThreadDelayService(),
                preferences);

        ConsoleBoardRenderer board_renderer = new(
            writer,
            theme);
        ConsoleGameInput game_input = new(reader, writer);
        IVisualFeedbackService visual_feedback =
            new VisualFeedbackService(board_renderer);

        ConsoleGameOutput game_output = new(
            writer,
            board_renderer,
            theme,
            art_catalog,
            visual_feedback);

        IMatchSessionRunner match_session_runner =
            new ConsoleMatchSessionRunner(
                game_input,
                game_output,
                animation_service);

        CitationMetadata citation_metadata =
            new CitationMetadataLoader().load(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "CITATION.cff"));

        IScreen[] screens =
        [
            new SplashScreen(
                reader,
                writer,
                theme,
                art_catalog,
                animation_service),
            new MainMenuScreen(reader, writer),
            new MatchSetupScreen(reader, writer),
            new PlayingScreen(match_session_runner),
            new MatchResultScreen(reader, writer),
            new StatisticsScreen(reader, writer),
            new ExperimentSetupScreen(reader, writer),
            new SettingsScreen(reader, writer),
            new HelpScreen(reader, writer),
            new CreditsScreen(
                reader,
                writer,
                citation_metadata),
            new ExitScreen(writer)
        ];

        ScreenManager screen_manager = new(screens);
        ScreenContext context = new(preferences);

        screen_manager.run(
            ScreenState.Splash,
            context);
    }
}
