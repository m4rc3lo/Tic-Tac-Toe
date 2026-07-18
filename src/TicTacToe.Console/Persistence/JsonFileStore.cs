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

            string json = File.ReadAllText(
                path,
                Encoding.UTF8);

            return JsonSerializer.Deserialize<T>(
                       json,
                       serializer_options)
                   ?? default_value;
        }
        catch (JsonException)
        {
            return default_value;
        }
        catch (IOException)
        {
            return default_value;
        }
        catch (UnauthorizedAccessException)
        {
            return default_value;
        }
    }

    public void save<T>(
        string path,
        T value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(value);

        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string temporary_path =
            $"{path}.tmp-{Guid.NewGuid():N}";

        try
        {
            string json = JsonSerializer.Serialize(
                value,
                serializer_options);

            File.WriteAllText(
                temporary_path,
                json,
                new UTF8Encoding(
                    encoderShouldEmitUTF8Identifier: false));

            File.Move(
                temporary_path,
                path,
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
}
