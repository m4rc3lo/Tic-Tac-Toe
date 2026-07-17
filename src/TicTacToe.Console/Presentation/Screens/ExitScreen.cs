using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Finaliza a navegação da aplicação.
/// </summary>
public sealed class ExitScreen : IScreen
{
    private readonly TextWriter writer;

    public ExitScreen(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    public ScreenState State => ScreenState.Exit;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine("Aplicação encerrada.");
        return new ScreenTransition(ScreenState.Exit);
    }
}
