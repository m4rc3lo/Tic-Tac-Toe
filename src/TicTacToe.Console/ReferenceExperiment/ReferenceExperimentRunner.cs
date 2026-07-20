using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using TicTacToe.Application;
using TicTacToe.Persistence;
using TicTacToe.Persistence.Csv;

namespace TicTacToe.ReferenceExperiment;

/// <summary>
/// Executa todos os cenários do experimento de referência sem apresentação.
/// </summary>
public sealed class ReferenceExperimentRunner
{
    public ReferenceExperimentManifest run(
        ReferenceExperimentPlan plan,
        string commit)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentException.ThrowIfNullOrWhiteSpace(commit);

        string version = Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString() ?? "unknown";
        DateTimeOffset started_at = DateTimeOffset.UtcNow;
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<ReferenceScenarioArtifact> artifacts = [];

        foreach (ReferenceExperimentScenario scenario in plan.get_scenarios())
        {
            string scenario_directory = Path.Combine(
                plan.OutputDirectory,
                scenario.Id);
            IExperimentResultRepository[] repositories =
            [
                new JsonExperimentResultRepository(),
                new CsvExperimentResultRepository(new CsvWriter())
            ];
            ExperimentController controller = new(
                new ExperimentStrategyFactory(),
                new SequentialSeedSequence(),
                new StopwatchExperimentTimerFactory(),
                repositories);
            ExperimentResult result = controller.run(
                new ExperimentConfiguration(
                scenario.XStrategy,
                scenario.OStrategy,
                plan.RepetitionsPerScenario,
                plan.AlternateSymbols,
                plan.AlternateFirstParticipant,
                plan.BaseSeed,
                version,
                scenario_directory,
                ContinueOnFailure: true,
                PersistMatchesToHistory: false));

            persist_final_result(
                scenario.Id,
                scenario_directory,
                result,
                repositories);

            string json_path = Path.Combine(
                scenario_directory,
                "experiment-result.json");
            string csv_path = Path.Combine(
                scenario_directory,
                "experiment-metrics.csv");

            validate_scenario_artifacts(
                scenario.Id,
                result,
                json_path,
                csv_path);

            artifacts.Add(new ReferenceScenarioArtifact(
                scenario.Id,
                Path.GetRelativePath(plan.OutputDirectory, scenario_directory),
                Path.GetFileName(json_path),
                calculate_sha256(json_path),
                Path.GetFileName(csv_path),
                calculate_sha256(csv_path)));
        }

        stopwatch.Stop();
        ReferenceEnvironmentRecord environment = new(
            version,
            commit,
            RuntimeInformation.OSDescription,
            RuntimeInformation.FrameworkDescription,
            RuntimeInformation.ProcessArchitecture.ToString(),
            Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ??
                Environment.GetEnvironmentVariable("HOSTTYPE") ??
                "unavailable",
            started_at,
            started_at + stopwatch.Elapsed,
            stopwatch.ElapsedMilliseconds);
        ReferenceExperimentManifest manifest = new(
            plan,
            environment,
            Array.AsReadOnly(artifacts.ToArray()));

        Directory.CreateDirectory(plan.OutputDirectory);
        File.WriteAllText(
            Path.Combine(plan.OutputDirectory, "reference-manifest.json"),
            JsonSerializer.Serialize(
                manifest,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                }));
        return manifest;
    }



    private static void persist_final_result(
        string scenario_id,
        string scenario_directory,
        ExperimentResult result,
        IReadOnlyList<IExperimentResultRepository> repositories)
    {
        const int max_attempts = 3;

        foreach (IExperimentResultRepository repository in repositories)
        {
            Exception? last_exception = null;

            for (int attempt = 1; attempt <= max_attempts; attempt++)
            {
                try
                {
                    repository.save(
                        scenario_directory,
                        result);
                    last_exception = null;
                    break;
                }
                catch (Exception exception)
                    when (is_expected_persistence_failure(exception))
                {
                    last_exception = exception;

                    if (attempt < max_attempts)
                    {
                        Thread.Sleep(
                            TimeSpan.FromMilliseconds(50 * attempt));
                    }
                }
            }

            if (last_exception is not null)
            {
                throw new InfrastructureOperationException(
                    "persistir resultado final do experimento de referência",
                    $"O cenário '{scenario_id}' terminou, mas o resultado final " +
                    $"não pôde ser persistido por " +
                    $"{repository.GetType().Name}.",
                    last_exception);
            }
        }
    }

    private static bool is_expected_persistence_failure(
        Exception exception)
    {
        return exception is InfrastructureOperationException
            or IOException
            or UnauthorizedAccessException;
    }

    private static void validate_scenario_artifacts(
        string scenario_id,
        ExperimentResult result,
        string json_path,
        string csv_path)
    {
        if (!File.Exists(json_path))
        {
            throw new InvalidDataException(
                $"O cenário '{scenario_id}' não produziu o arquivo JSON.");
        }

        if (!File.Exists(csv_path))
        {
            throw new InvalidDataException(
                $"O cenário '{scenario_id}' não produziu o arquivo CSV.");
        }

        int expected_count = result.Summary.TotalRuns;
        int metric_count = result.Metrics.Count;
        int csv_count = count_csv_records(csv_path);

        using JsonDocument document =
            JsonDocument.Parse(File.ReadAllText(json_path));
        JsonElement root = document.RootElement;
        int json_summary_count = root
            .GetProperty("summary")
            .GetProperty("totalRuns")
            .GetInt32();
        int json_metric_count = root
            .GetProperty("metrics")
            .GetArrayLength();

        if (expected_count != metric_count ||
            expected_count != json_summary_count ||
            expected_count != json_metric_count ||
            expected_count != csv_count)
        {
            throw new InvalidDataException(
                $"O cenário '{scenario_id}' gerou artefatos inconsistentes. " +
                $"Resultado em memória: {expected_count}; " +
                $"métricas em memória: {metric_count}; " +
                $"resumo JSON: {json_summary_count}; " +
                $"métricas JSON: {json_metric_count}; " +
                $"linhas CSV: {csv_count}.");
        }
    }

    private static int count_csv_records(string path)
    {
        string content = File.ReadAllText(path);
        int record_count = 0;
        bool inside_quotes = false;

        for (int index = 0; index < content.Length; index++)
        {
            char current = content[index];

            if (current == '"')
            {
                if (inside_quotes &&
                    index + 1 < content.Length &&
                    content[index + 1] == '"')
                {
                    index++;
                    continue;
                }

                inside_quotes = !inside_quotes;
                continue;
            }

            if (!inside_quotes && current == '\n')
            {
                record_count++;
            }
        }

        if (content.Length > 0 &&
            content[^1] != '\n')
        {
            record_count++;
        }

        return Math.Max(0, record_count - 1);
    }

    private static string calculate_sha256(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }
}
