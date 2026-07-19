namespace TicTacToe.AI;

/// <summary>
/// Expõe métricas opcionais produzidas pela última seleção de jogada.
/// </summary>
public interface ISearchMetricsProvider
{
    /// <summary>
    /// Obtém a quantidade de estados avaliados na última busca.
    /// </summary>
    long LastEvaluatedStates { get; }
}
