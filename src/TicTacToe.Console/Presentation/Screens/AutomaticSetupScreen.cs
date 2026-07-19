using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Coleta as opções do modo demonstrativo IA contra IA.
/// </summary>
public sealed class AutomaticSetupScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public AutomaticSetupScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);
        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.AutomaticSetup;

    public ScreenTransition show(ScreenContext context)
    {
        StrategyKind default_strategy =
            StrategyKindParser.parse_or_default(
                context.ApplicationSettings.DefaultStrategy);

        StrategyKind? x_strategy = read_strategy(
            "Strategy de X",
            default_strategy);

        if (!x_strategy.HasValue)
        {
            return new ScreenTransition(ScreenState.MainMenu);
        }

        StrategyKind? o_strategy = read_strategy(
            "Strategy de O",
            default_strategy);

        if (!o_strategy.HasValue)
        {
            return new ScreenTransition(ScreenState.MainMenu);
        }

        int? seed = read_seed(
            context.ApplicationSettings.RandomSeed);
        bool persist_match = read_boolean(
            "Persistir a partida no histórico? [s/N]: ");

        context.set_automatic_match_configuration(
            new AutomaticMatchConfiguration(
                x_strategy.Value,
                o_strategy.Value,
                seed,
                persist_match));

        return new ScreenTransition(
            ScreenState.AutomaticPlaying);
    }

    private StrategyKind? read_strategy(
        string label,
        StrategyKind default_strategy)
    {
        while (true)
        {
            writer.WriteLine();
            writer.WriteLine(
                $"{label} [padrão: {default_strategy}]:");
            writer.WriteLine("1 - Random");
            writer.WriteLine("2 - Heuristic");
            writer.WriteLine("3 - Minimax");
            writer.WriteLine("0 - Voltar");
            writer.Write("Opção: ");

            string? input = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return default_strategy;
            }

            if (input.Trim() == "0")
            {
                return null;
            }

            StrategyKind? strategy =
                StrategyKindParser.parse_option(input);

            if (strategy.HasValue)
            {
                return strategy;
            }

            writer.WriteLine("Strategy inválida.");
        }
    }

    private int? read_seed(int? default_seed)
    {
        while (true)
        {
            writer.Write(
                $"Semente [padrão: {format_seed(default_seed)}; - para nenhuma]: ");
            string? input = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return default_seed;
            }

            if (input.Trim() == "-")
            {
                return null;
            }

            if (int.TryParse(input.Trim(), out int seed))
            {
                return seed;
            }

            writer.WriteLine("Semente inválida.");
        }
    }

    private bool read_boolean(string prompt)
    {
        writer.Write(prompt);
        string? input = reader.ReadLine();
        return string.Equals(
            input?.Trim(),
            "s",
            StringComparison.OrdinalIgnoreCase);
    }

    private static string format_seed(int? seed)
    {
        return seed?.ToString() ?? "não definida";
    }
}
