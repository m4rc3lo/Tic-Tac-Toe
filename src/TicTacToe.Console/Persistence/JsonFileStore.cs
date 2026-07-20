
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicTacToe.Persistence;

/// <summary>
/// Centraliza leitura segura e substituição atômica de arquivos JSON.
/// </summary>
internal sealed class JsonFileStore
{
    private readonly JsonSerializerOptions serializer_options;

    public JsonFileStore()
    {
        serializer_options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            UnmappedMemberHandling =
                JsonUnmappedMemberHandling.Skip
        };
    }

    public T load_or_default<T>(
        string path,
        T default_value)
    {
        try
        {
            if (!File.Exists(path))
            {
                return default_value;
            }

            string json = File.ReadAllText(path, Encoding.UTF8);
            T? value = JsonSerializer.Deserialize<T>(
                json,
                serializer_options);

            return value is null
                ? quarantine_and_return_default(path, default_value)
                : value;
        }
        catch (JsonException)
        {
            return quarantine_and_return_default(path, default_value);
        }
        catch (IOException exception)
        {
            throw create_exception("ler JSON", exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw create_exception("ler JSON", exception);
        }
    }

    public void save<T>(
        string path,
        T value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(value);

        string? directory = Path.GetDirectoryName(path);
        string temporary_path =
            $"{path}.tmp-{Guid.NewGuid():N}";

        try
        {
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(
                value,
                serializer_options);

            File.WriteAllText(
                temporary_path,
                json,
                new UTF8Encoding(false));

            File.Move(
                temporary_path,
                path,
                overwrite: true);
        }
        catch (IOException exception)
        {
            throw create_exception("gravar JSON", exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw create_exception("gravar JSON", exception);
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

    private static T quarantine_and_return_default<T>(
        string path,
        T default_value)
    {
        try
        {
            JsonCorruptionQuarantine.preserve(path);
            return default_value;
        }
        catch (IOException exception)
        {
            throw create_exception(
                "preservar JSON inválido",
                exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw create_exception(
                "preservar JSON inválido",
                exception);
        }
    }

    private static InfrastructureOperationException create_exception(
        string operation,
        Exception exception)
    {
        return new InfrastructureOperationException(
            operation,
            "Não foi possível acessar um arquivo de dados.",
            exception);
    }
}
