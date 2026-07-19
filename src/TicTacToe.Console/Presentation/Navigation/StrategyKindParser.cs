namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Converte nomes persistidos e opções de menu em StrategyKind.
/// </summary>
public static class StrategyKindParser
{
    public static StrategyKind parse_or_default(
        string? value,
        StrategyKind default_value = StrategyKind.Minimax)
    {
        return Enum.TryParse(
            value,
            ignoreCase: true,
            out StrategyKind parsed)
            ? parsed
            : default_value;
    }

    public static StrategyKind? parse_option(string? value)
    {
        return value?.Trim() switch
        {
            "1" => StrategyKind.Random,
            "2" => StrategyKind.Heuristic,
            "3" => StrategyKind.Minimax,
            _ => null
        };
    }
}
