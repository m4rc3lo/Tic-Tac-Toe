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
            writer.WriteLine("1 - Jogar contra IA");
            writer.WriteLine("2 - Demonstração IA contra IA");
            writer.WriteLine("3 - Estatísticas");
            writer.WriteLine("4 - Experimentos");
            writer.WriteLine("5 - Configurações");
            writer.WriteLine("6 - Ajuda");
            writer.WriteLine("7 - Créditos");
            writer.WriteLine("0 - Sair");
            writer.Write("Opção: ");

            ScreenState? target = reader.ReadLine()?.Trim() switch
            {
                "1" => ScreenState.MatchSetup,
                "2" => ScreenState.AutomaticSetup,
                "3" => ScreenState.Statistics,
                "4" => ScreenState.ExperimentSetup,
                "5" => ScreenState.Settings,
                "6" => ScreenState.Help,
                "7" => ScreenState.Credits,
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
