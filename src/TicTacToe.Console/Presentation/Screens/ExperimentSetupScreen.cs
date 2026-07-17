using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta a configuração futura de experimentos.
/// </summary>
public sealed class ExperimentSetupScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public ExperimentSetupScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.ExperimentSetup;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine();
        writer.WriteLine("Modo experimental ainda não disponível.");
        writer.WriteLine("Pressione Enter para voltar ao menu.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
