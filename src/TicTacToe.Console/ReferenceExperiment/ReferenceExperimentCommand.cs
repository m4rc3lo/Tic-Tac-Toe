namespace TicTacToe.ReferenceExperiment;

/// <summary>
/// Interpreta o comando não interativo do experimento de referência.
/// </summary>
public static class ReferenceExperimentCommand
{
    public static bool try_run(string[] args, TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(args);
        ArgumentNullException.ThrowIfNull(writer);

        if (!args.Contains("--reference-experiment", StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        string output_directory = get_value(args, "--output") ??
            Path.Combine("artifacts", "experiments", "reference");
        string commit = get_value(args, "--commit") ?? "unrecorded";
        ReferenceExperimentPlan plan =
            ReferenceExperimentPlan.create_default(output_directory);
        ReferenceExperimentManifest manifest =
            new ReferenceExperimentRunner().run(plan, commit);

        writer.WriteLine($"Experimento concluído: {manifest.Scenarios.Count} cenários.");
        writer.WriteLine($"Manifesto: {Path.Combine(output_directory, "reference-manifest.json")}");
        return true;
    }

    private static string? get_value(string[] args, string option)
    {
        int index = Array.FindIndex(
            args,
            item => string.Equals(item, option, StringComparison.OrdinalIgnoreCase));
        return index >= 0 && index + 1 < args.Length
            ? args[index + 1]
            : null;
    }
}
