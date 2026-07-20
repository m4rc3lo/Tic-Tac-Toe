
using System.Globalization;

namespace TicTacToe.Persistence;

/// <summary>
/// Preserva arquivos JSON inválidos antes da recuperação.
/// </summary>
internal static class JsonCorruptionQuarantine
{
    public static string? preserve(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        string timestamp = DateTimeOffset.UtcNow.ToString(
            "yyyyMMddTHHmmssfffZ",
            CultureInfo.InvariantCulture);
        string quarantine_path =
            $"{path}.corrupt-{timestamp}-{Guid.NewGuid():N}";

        File.Move(path, quarantine_path);
        return quarantine_path;
    }
}
