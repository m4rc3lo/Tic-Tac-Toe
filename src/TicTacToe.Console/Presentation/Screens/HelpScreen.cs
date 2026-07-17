using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta orientações básicas de uso.
/// </summary>
public sealed class HelpScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public HelpScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.Help;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine();
        writer.WriteLine("Informe linha e coluna de 1 a 3 durante uma partida.");
        writer.WriteLine("Pressione Enter para voltar ao menu.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
