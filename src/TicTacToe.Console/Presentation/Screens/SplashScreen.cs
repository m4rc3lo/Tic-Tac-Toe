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
    private readonly IAnimationService animation_service;

    public SplashScreen(
        TextReader reader,
        TextWriter writer,
        ConsoleTheme theme,
        AsciiArtCatalog art_catalog,
        IAnimationService animation_service)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(theme);
        ArgumentNullException.ThrowIfNull(art_catalog);
        ArgumentNullException.ThrowIfNull(animation_service);

        this.reader = reader;
        this.writer = writer;
        this.theme = theme;
        this.art_catalog = art_catalog;
        this.animation_service = animation_service;
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
                animation_service.write_progressive(
                    theme.colorize_accent(line),
                    TimeSpan.FromMilliseconds(
                        theme.Preferences.AnimationDelayMilliseconds));
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
