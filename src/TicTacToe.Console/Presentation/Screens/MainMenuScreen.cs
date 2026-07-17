using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta as opções principais de navegação.
/// </summary>
public sealed class MainMenuScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public MainMenuScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.MainMenu;

    public ScreenTransition show(ScreenContext context)
    {
        while (true)
        {
            writer.WriteLine();
            writer.WriteLine("Menu principal");
            writer.WriteLine("1 - Jogar");
            writer.WriteLine("2 - Estatísticas");
            writer.WriteLine("3 - Experimentos");
            writer.WriteLine("4 - Configurações");
            writer.WriteLine("5 - Ajuda");
            writer.WriteLine("0 - Sair");
            writer.Write("Opção: ");

            string? option = reader.ReadLine();

            ScreenState? target = option?.Trim() switch
            {
                "1" => ScreenState.MatchSetup,
                "2" => ScreenState.Statistics,
                "3" => ScreenState.ExperimentSetup,
                "4" => ScreenState.Settings,
                "5" => ScreenState.Help,
                "0" => ScreenState.Exit,
                _ => null
            };

            if (target.HasValue)
            {
                return new ScreenTransition(target.Value);
            }

            writer.WriteLine("Opção inválida.");
        }
    }
}
