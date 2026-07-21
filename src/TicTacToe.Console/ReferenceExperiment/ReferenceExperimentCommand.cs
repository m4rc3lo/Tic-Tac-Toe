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

        if (!args.Contains(
                "--reference-experiment",
                StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        string output_directory =
            get_value(args, "--output") ??
            Path.Combine(
                "artifacts",
                "experiments",
                "reference");
        string commit =
            get_value(args, "--commit") ??
            "unrecorded";
        ReferenceExperimentPlan default_plan =
            ReferenceExperimentPlan.create_default(
                output_directory);
        int repetitions = get_positive_integer(
            args,
            "--repetitions",
            default_plan.RepetitionsPerScenario);
        int base_seed = get_integer(
            args,
            "--base-seed",
            default_plan.BaseSeed);
        ReferenceExperimentPlan plan = default_plan with
        {
            RepetitionsPerScenario = repetitions,
            BaseSeed = base_seed
        };
        ReferenceExperimentManifest manifest =
            new ReferenceExperimentRunner().run(
                plan,
                commit);

        writer.WriteLine(
            $"Experimento concluído: " +
            $"{manifest.Scenarios.Count} cenários.");
        writer.WriteLine(
            $"Repetições por cenário: " +
            $"{plan.RepetitionsPerScenario}.");
        writer.WriteLine(
            $"Semente base: {plan.BaseSeed}.");
        writer.WriteLine(
            $"Manifesto: " +
            $"{Path.Combine(output_directory, "reference-manifest.json")}");
        return true;
    }

    private static string? get_value(
        string[] args,
        string option)
    {
        int index = Array.FindIndex(
            args,
            item => string.Equals(
                item,
                option,
                StringComparison.OrdinalIgnoreCase));

        return index >= 0 &&
            index + 1 < args.Length
            ? args[index + 1]
            : null;
    }

    private static int get_positive_integer(
        string[] args,
        string option,
        int default_value)
    {
        int value = get_integer(
            args,
            option,
            default_value);

        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(
                option,
                "O valor deve ser maior que zero.");
        }

        return value;
    }

    private static int get_integer(
        string[] args,
        string option,
        int default_value)
    {
        string? raw_value = get_value(
            args,
            option);

        if (raw_value is null)
        {
            return default_value;
        }

        if (!int.TryParse(
                raw_value,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture,
                out int value))
        {
            throw new ArgumentException(
                $"O valor de {option} deve ser inteiro.",
                option);
        }

        return value;
    }
}
