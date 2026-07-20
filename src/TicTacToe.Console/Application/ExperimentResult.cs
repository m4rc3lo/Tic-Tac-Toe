
using TicTacToe.Persistence;

namespace TicTacToe.Application;

/// <summary>
/// Contém métricas individuais e agregadas de um experimento.
/// </summary>
public sealed record ExperimentResult
{
    private IReadOnlyList<ExperimentMetricRecord> metrics =
        Array.Empty<ExperimentMetricRecord>();

    public ExperimentResult()
    {
    }

    public ExperimentResult(
        Guid experiment_id,
        ExperimentConfiguration configuration,
        IEnumerable<ExperimentMetricRecord> metrics,
        ExperimentSummary summary)
    {
        ExperimentId = experiment_id;
        Configuration = configuration;
        Metrics = metrics.ToArray();
        Summary = summary;
    }

    public Guid ExperimentId { get; init; }
    public ExperimentConfiguration Configuration { get; init; } = null!;

    public IReadOnlyList<ExperimentMetricRecord> Metrics
    {
        get => metrics;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            metrics = Array.AsReadOnly(value.ToArray());
        }
    }

    public ExperimentSummary Summary { get; init; } = null!;
}
