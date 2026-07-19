namespace TicTacToe.Application;

/// <summary>
/// Consolida resultados do lote experimental.
/// </summary>
public sealed record ExperimentSummary(
    int TotalRuns,
    int SuccessfulRuns,
    int FailedRuns,
    int XWins,
    int OWins,
    int Draws,
    double AverageMoves,
    double AverageDurationMilliseconds);
