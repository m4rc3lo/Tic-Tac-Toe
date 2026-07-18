namespace TicTacToe.Persistence;

/// <summary>
/// Representa uma observação produzida por uma execução experimental.
/// </summary>
public sealed record ExperimentMetricRecord(
    Guid ExperimentId,
    int RunNumber,
    string XStrategy,
    string OStrategy,
    int? Seed,
    string Result,
    int MoveCount,
    long DurationMilliseconds,
    long? EvaluatedStates,
    bool Failed,
    string? FailureMessage,
    string ApplicationVersion);
