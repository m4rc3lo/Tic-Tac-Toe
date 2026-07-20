namespace TicTacToe.ReferenceExperiment;

/// <summary>
/// Consolida protocolo, ambiente e artefatos do experimento de referência.
/// </summary>
public sealed record ReferenceExperimentManifest(
    ReferenceExperimentPlan Plan,
    ReferenceEnvironmentRecord Environment,
    IReadOnlyList<ReferenceScenarioArtifact> Scenarios);

/// <summary>
/// Identifica os arquivos e hashes produzidos por um cenário.
/// </summary>
public sealed record ReferenceScenarioArtifact(
    string ScenarioId,
    string Directory,
    string JsonFile,
    string JsonSha256,
    string CsvFile,
    string CsvSha256);
