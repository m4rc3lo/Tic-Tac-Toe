using TicTacToe.Persistence;

namespace TicTacToe.Application;

/// <summary>
/// Contém métricas individuais e agregadas de um experimento.
/// </summary>
public sealed record ExperimentResult(
    Guid ExperimentId,
    ExperimentConfiguration Configuration,
    IReadOnlyList<ExperimentMetricRecord> Metrics,
    ExperimentSummary Summary);
