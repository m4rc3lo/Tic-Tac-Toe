namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Contém as opções do modo demonstrativo IA contra IA.
/// </summary>
public sealed record AutomaticMatchConfiguration(
    StrategyKind XStrategy,
    StrategyKind OStrategy,
    int? RandomSeed,
    bool PersistMatch);
