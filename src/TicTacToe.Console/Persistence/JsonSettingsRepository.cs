using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicTacToe.Persistence;

/// <summary>
/// Persiste configurações JSON com recuperação segura e gravação atômica.
/// </summary>
public sealed class JsonSettingsRepository : ISettingsRepository
{
    private readonly string settings_path;
    private readonly SettingsValidator validator;
    private readonly JsonSerializerOptions serializer_options;

    /// <summary>
    /// Inicializa o repositório para o arquivo informado.
    /// </summary>
    public JsonSettingsRepository(
        string settings_path,
        SettingsValidator validator)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settings_path);
        ArgumentNullException.ThrowIfNull(validator);

        this.settings_path = Path.GetFullPath(settings_path);
        this.validator = validator;
        serializer_options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };
    }

    /// <inheritdoc />
    public ApplicationSettings load()
    {
        if (!File.Exists(settings_path))
        {
            ApplicationSettings defaults =
                ApplicationSettings.create_default();
            save(defaults);
            return defaults;
        }

        try
        {
            string json = File.ReadAllText(settings_path);
            ApplicationSettings? settings =
                JsonSerializer.Deserialize<ApplicationSettings>(
                    json,
                    serializer_options);

            if (settings is null)
            {
                return recover_defaults();
            }

            SettingsValidationResult result =
                validator.validate(settings);

            return result.IsValid
                ? settings
                : recover_defaults();
        }
        catch (JsonException)
        {
            return recover_defaults();
        }
        catch (IOException)
        {
            return ApplicationSettings.create_default();
        }
        catch (UnauthorizedAccessException)
        {
            return ApplicationSettings.create_default();
        }
    }

    /// <inheritdoc />
    public void save(ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        SettingsValidationResult validation =
            validator.validate(settings);

        if (!validation.IsValid)
        {
            throw new ArgumentException(
                string.Join(" ", validation.Errors),
                nameof(settings));
        }

        string? directory = Path.GetDirectoryName(settings_path);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string temporary_path =
            settings_path + ".tmp-" + Guid.NewGuid().ToString("N");

        try
        {
            string json = JsonSerializer.Serialize(
                settings,
                serializer_options);

            File.WriteAllText(temporary_path, json);
            File.Move(
                temporary_path,
                settings_path,
                overwrite: true);
        }
        finally
        {
            if (File.Exists(temporary_path))
            {
                File.Delete(temporary_path);
            }
        }
    }

    private ApplicationSettings recover_defaults()
    {
        ApplicationSettings defaults =
            ApplicationSettings.create_default();

        try
        {
            save(defaults);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }

        return defaults;
    }
}
