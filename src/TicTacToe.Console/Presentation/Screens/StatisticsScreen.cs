using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta a área reservada para estatísticas.
/// </summary>
public sealed class StatisticsScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public StatisticsScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.Statistics;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine();
        writer.WriteLine("Estatísticas ainda não disponíveis.");
        writer.WriteLine("Pressione Enter para voltar ao menu.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
