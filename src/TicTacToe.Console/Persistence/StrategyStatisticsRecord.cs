namespace TicTacToe.Persistence;

/// <summary>
/// Consolida estatísticas de uma Strategy computacional.
/// </summary>
public sealed record StrategyStatisticsRecord(
    string Strategy,
    int Matches,
    int Wins,
    int Losses,
    int Draws);
