
using System.Text;
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
            string json = File.ReadAllText(
                settings_path,
                Encoding.UTF8);
            ApplicationSettings? settings =
                JsonSerializer.Deserialize<ApplicationSettings>(
                    json,
                    serializer_options);

            if (settings is null ||
                !validator.validate(settings).IsValid)
            {
                return recover_invalid_file();
            }

            return settings;
        }
        catch (JsonException)
        {
            return recover_invalid_file();
        }
        catch (IOException exception)
        {
            throw create_exception("ler configurações", exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw create_exception("ler configurações", exception);
        }
    }

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
        string temporary_path =
            settings_path + ".tmp-" + Guid.NewGuid().ToString("N");

        try
        {
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(
                settings,
                serializer_options);

            File.WriteAllText(
                temporary_path,
                json,
                new UTF8Encoding(false));
            File.Move(
                temporary_path,
                settings_path,
                overwrite: true);
        }
        catch (IOException exception)
        {
            throw create_exception("gravar configurações", exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw create_exception("gravar configurações", exception);
        }
        finally
        {
            try
            {
                if (File.Exists(temporary_path))
                {
                    File.Delete(temporary_path);
                }
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }

    private ApplicationSettings recover_invalid_file()
    {
        try
        {
            JsonCorruptionQuarantine.preserve(settings_path);
        }
        catch (IOException exception)
        {
            throw create_exception(
                "preservar configurações inválidas",
                exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw create_exception(
                "preservar configurações inválidas",
                exception);
        }

        ApplicationSettings defaults =
            ApplicationSettings.create_default();
        save(defaults);
        return defaults;
    }

    private static InfrastructureOperationException create_exception(
        string operation,
        Exception exception)
    {
        return new InfrastructureOperationException(
            operation,
            "Não foi possível acessar as configurações da aplicação.",
            exception);
    }
}
