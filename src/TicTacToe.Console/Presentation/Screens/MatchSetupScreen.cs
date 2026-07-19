using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Coleta a configuração de uma partida entre pessoa e IA.
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
        StrategyKind default_strategy =
            StrategyKindParser.parse_or_default(
                context.ApplicationSettings.DefaultStrategy);

        while (true)
        {
            writer.WriteLine(
                $"Strategy adversária [padrão: {default_strategy}]:");
            writer.WriteLine("1 - Random");
            writer.WriteLine("2 - Heuristic");
            writer.WriteLine("3 - Minimax");
            writer.WriteLine("0 - Voltar");
            writer.Write("Opção: ");

            string? option = reader.ReadLine();

            if (option?.Trim() == "0")
            {
                return new ScreenTransition(ScreenState.MainMenu);
            }

            StrategyKind? strategy_kind =
                string.IsNullOrWhiteSpace(option)
                    ? default_strategy
                    : StrategyKindParser.parse_option(option);

            if (!strategy_kind.HasValue)
            {
                writer.WriteLine("Strategy inválida.");
                continue;
            }

            context.set_match_configuration(
                new MatchConfiguration(
                    player_name,
                    strategy_kind.Value,
                    context.ApplicationSettings.RandomSeed));

            return new ScreenTransition(ScreenState.Playing);
        }
    }
}
