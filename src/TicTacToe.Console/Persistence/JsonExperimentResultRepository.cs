using TicTacToe.Application;

namespace TicTacToe.Persistence;

/// <summary>
/// Persiste resultados experimentais acumulados em JSON.
/// </summary>
public sealed class JsonExperimentResultRepository
    : IExperimentResultRepository
{
    private readonly JsonFileStore store = new();

    public void save(string output_directory, ExperimentResult result)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(output_directory);
        ArgumentNullException.ThrowIfNull(result);

        store.save(
            Path.Combine(output_directory, "experiment-result.json"),
            result);
    }
}
