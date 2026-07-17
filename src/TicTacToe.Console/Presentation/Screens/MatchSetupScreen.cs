using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Coleta a configuração mínima de uma partida.
/// </summary>
public sealed class MatchSetupScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public MatchSetupScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.MatchSetup;

    public ScreenTransition show(ScreenContext context)
    {
        writer.WriteLine();
        writer.Write("Nome da pessoa [Pessoa]: ");

        string? name_input = reader.ReadLine();
        string player_name = string.IsNullOrWhiteSpace(name_input)
            ? "Pessoa"
            : name_input.Trim();

        while (true)
        {
            writer.WriteLine("Estratégia adversária:");
            writer.WriteLine("1 - Aleatória");
            writer.WriteLine("2 - Heurística");
            writer.WriteLine("3 - Minimax");
            writer.WriteLine("0 - Voltar");
            writer.Write("Opção: ");

            string? option = reader.ReadLine();

            if (option?.Trim() == "0")
            {
                return new ScreenTransition(ScreenState.MainMenu);
            }

            StrategyKind? strategy_kind = option?.Trim() switch
            {
                "1" => StrategyKind.Random,
                "2" => StrategyKind.Heuristic,
                "3" => StrategyKind.Minimax,
                _ => null
            };

            if (!strategy_kind.HasValue)
            {
                writer.WriteLine("Estratégia inválida.");
                continue;
            }

            context.set_match_configuration(
                new MatchConfiguration(
                    player_name,
                    strategy_kind.Value));

            return new ScreenTransition(ScreenState.Playing);
        }
    }
}
