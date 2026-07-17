namespace TicTacToe.Presentation;

/// <summary>
/// Centraliza caracteres, cores e operações dependentes das preferências visuais.
/// </summary>
public sealed class ConsoleTheme
{
    private const string Reset = "\u001b[0m";
    private const string Accent = "\u001b[36m";
    private const string Success = "\u001b[32m";
    private const string Warning = "\u001b[33m";
    private const string Error = "\u001b[31m";

    public ConsoleTheme(PresentationPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        Preferences = preferences;
    }

    public PresentationPreferences Preferences { get; }

    public string VerticalSeparator =>
        Preferences.UseUnicode ? "│" : "|";

    public string HorizontalSeparator =>
        Preferences.UseUnicode ? "───┼───┼───" : "---+---+---";

    public string colorize_accent(string text)
    {
        return colorize(text, Accent);
    }

    public string colorize_success(string text)
    {
        return colorize(text, Success);
    }

    public string colorize_warning(string text)
    {
        return colorize(text, Warning);
    }

    public string colorize_error(string text)
    {
        return colorize(text, Error);
    }

    public void clear(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (Preferences.ClearScreen)
        {
            writer.Write("\u001b[2J\u001b[H");
        }
    }

    private string colorize(string text, string color)
    {
        return Preferences.UseAnsiColors
            ? $"{color}{text}{Reset}"
            : text;
    }
}
