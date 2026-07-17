using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta a área reservada para configurações.
/// </summary>
public sealed class SettingsScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public SettingsScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.Settings;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine();
        writer.WriteLine("Configurações persistentes ainda não disponíveis.");
        writer.WriteLine("Pressione Enter para voltar ao menu.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
