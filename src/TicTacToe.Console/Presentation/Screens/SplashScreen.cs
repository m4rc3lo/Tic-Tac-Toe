using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta a identificação inicial da aplicação.
/// </summary>
public sealed class SplashScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public SplashScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.Splash;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine("Tic-Tac-Toe Console AI");
        writer.WriteLine("Pressione Enter para continuar.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
