using TicTacToe.Domain;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Representa o resultado de uma demonstração automática.
/// </summary>
public sealed record AutomaticMatchResult(
    Match Match,
    bool WasCancelled,
    int? EffectiveSeed);
