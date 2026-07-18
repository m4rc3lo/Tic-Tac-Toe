namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Contém as opções coletadas para iniciar uma partida.
/// </summary>
/// <param name="PlayerName">Nome da pessoa participante.</param>
/// <param name="OpponentStrategy">Estratégia do participante computacional.</param>
public sealed record MatchConfiguration(
    string PlayerName,
    StrategyKind OpponentStrategy,
    int? RandomSeed = null);
