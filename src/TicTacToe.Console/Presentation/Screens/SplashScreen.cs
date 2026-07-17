using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta a identificação inicial da aplicação.
/// </summary>
public sealed class SplashScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;
    private readonly ConsoleTheme theme;
    private readonly AsciiArtCatalog art_catalog;

    public SplashScreen(
        TextReader reader,
        TextWriter writer,
        ConsoleTheme theme,
        AsciiArtCatalog art_catalog)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(theme);
        ArgumentNullException.ThrowIfNull(art_catalog);

        this.reader = reader;
        this.writer = writer;
        this.theme = theme;
        this.art_catalog = art_catalog;
    }

    public ScreenState State => ScreenState.Splash;

    public ScreenTransition show(ScreenContext context)
    {
        theme.clear(writer);

        if (theme.Preferences.VisualEffects)
        {
            foreach (string line in art_catalog.get_logo(
                         theme.Preferences.UseUnicode))
            {
                writer.WriteLine(
                    theme.colorize_accent(line));
            }
        }
        else
        {
            writer.WriteLine("Tic-Tac-Toe Console AI");
        }

        writer.WriteLine("Pressione Enter para continuar.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
