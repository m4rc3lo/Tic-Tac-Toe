namespace TicTacToe.Persistence;

/// <summary>
/// Valida valores reconhecidos antes do uso ou da persistência.
/// </summary>
public sealed class SettingsValidator
{
    private static readonly string[] supported_strategies =
        ["Random", "Heuristic", "Minimax"];

    /// <summary>
    /// Valida a configuração informada.
    /// </summary>
    public SettingsValidationResult validate(ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        List<string> errors = [];

        if (settings.AnimationDelayMilliseconds < 0 ||
            settings.AnimationDelayMilliseconds > 5000)
        {
            errors.Add(
                "AnimationDelayMilliseconds deve estar entre 0 e 5000.");
        }

        if (settings.Directories is null)
        {
            errors.Add("Directories deve ser informado.");
        }
        else
        {
            validate_directory(
                settings.Directories.Data,
                "Directories.Data",
                errors);
            validate_directory(
                settings.Directories.Exports,
                "Directories.Exports",
                errors);
        }

        if (string.IsNullOrWhiteSpace(settings.DefaultStrategy) ||
            !supported_strategies.Contains(
                settings.DefaultStrategy,
                StringComparer.OrdinalIgnoreCase))
        {
            errors.Add(
                "DefaultStrategy deve ser Random, Heuristic ou Minimax.");
        }

        return new SettingsValidationResult(
            errors.Count == 0,
            errors.AsReadOnly());
    }

    private static void validate_directory(
        string? directory,
        string property_name,
        ICollection<string> errors)
    {
        if (string.IsNullOrWhiteSpace(directory))
        {
            errors.Add($"{property_name} não pode ser vazio.");
            return;
        }

        if (Path.IsPathRooted(directory))
        {
            errors.Add(
                $"{property_name} deve ser relativo ao diretório da aplicação.");
        }
    }
}
