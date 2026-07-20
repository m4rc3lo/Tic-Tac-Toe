using TicTacToe.Application;

namespace TicTacToe.ReferenceExperiment;

/// <summary>
/// Define o protocolo versionado do experimento de referência.
/// </summary>
public sealed record ReferenceExperimentPlan(
    int RepetitionsPerScenario,
    int BaseSeed,
    bool AlternateSymbols,
    bool AlternateFirstParticipant,
    string OutputDirectory)
{
    public static ReferenceExperimentPlan create_default(string output_directory)
    {
        return new ReferenceExperimentPlan(
            100,
            20260720,
            true,
            true,
            output_directory);
    }

    public IReadOnlyList<ReferenceExperimentScenario> get_scenarios()
    {
        ReferenceExperimentScenario[] scenarios =
        [
            new(
                "random-random",
                ExperimentStrategy.Random,
                ExperimentStrategy.Random),
            new(
                "random-heuristic",
                ExperimentStrategy.Random,
                ExperimentStrategy.Heuristic),
            new(
                "random-minimax",
                ExperimentStrategy.Random,
                ExperimentStrategy.Minimax),
            new(
                "heuristic-heuristic",
                ExperimentStrategy.Heuristic,
                ExperimentStrategy.Heuristic),
            new(
                "heuristic-minimax",
                ExperimentStrategy.Heuristic,
                ExperimentStrategy.Minimax),
            new(
                "minimax-minimax",
                ExperimentStrategy.Minimax,
                ExperimentStrategy.Minimax)
        ];

        return Array.AsReadOnly(scenarios);
    }
}

/// <summary>
/// Identifica um cenário não redundante do experimento de referência.
/// </summary>
public sealed record ReferenceExperimentScenario(
    string Id,
    ExperimentStrategy XStrategy,
    ExperimentStrategy OStrategy);
