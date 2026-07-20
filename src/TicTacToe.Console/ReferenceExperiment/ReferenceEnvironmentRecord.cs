namespace TicTacToe.ReferenceExperiment;

/// <summary>
/// Registra o ambiente em que o experimento foi executado.
/// </summary>
public sealed record ReferenceEnvironmentRecord(
    string ApplicationVersion,
    string Commit,
    string OperatingSystem,
    string Runtime,
    string Architecture,
    string Processor,
    DateTimeOffset StartedAt,
    DateTimeOffset FinishedAt,
    long DurationMilliseconds);
