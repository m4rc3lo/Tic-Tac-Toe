
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;

namespace TicTacToe.Composition;

internal static class ScreenComposition
{
    public static IScreen[] create(
        TextReader reader,
        TextWriter writer,
        PresentationComponents presentation)
    {
        CitationMetadata citation_metadata =
            new CitationMetadataLoader().load(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "CITATION.cff"));

        return
        [
            new SplashScreen(
                reader,
                writer,
                presentation.Theme,
                presentation.ArtCatalog,
                presentation.AnimationService),
            new MainMenuScreen(reader, writer),
            new MatchSetupScreen(reader, writer),
            new PlayingScreen(presentation.MatchRunner),
            new MatchResultScreen(reader, writer),
            new AutomaticSetupScreen(reader, writer),
            new AutomaticPlayingScreen(
                presentation.AutomaticRunner),
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
    }
}
