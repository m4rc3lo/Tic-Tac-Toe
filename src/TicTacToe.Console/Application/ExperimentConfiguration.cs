namespace TicTacToe.Application;

/// <summary>
/// Define um lote experimental reproduzível.
/// </summary>
public sealed record ExperimentConfiguration(
    ExperimentStrategy XStrategy,
    ExperimentStrategy OStrategy,
    int MatchCount,
    bool AlternateSymbols,
    bool AlternateFirstPlayer,
    int? BaseSeed,
    string ApplicationVersion,
    string OutputDirectory,
    bool ContinueOnFailure = true,
    bool PersistMatchesToHistory = false);
