
using System.Text;

namespace TicTacToe.Compatibility;

/// <summary>
/// Detecta capacidades do Console sem presumir suporte pela plataforma.
/// </summary>
public sealed class SystemConsoleCapabilityDetector
    : IConsoleCapabilityDetector
{
    private readonly Func<bool> is_input_redirected;
    private readonly Func<bool> is_output_redirected;
    private readonly Func<string?> get_term;
    private readonly Func<string?> get_no_color;
    private readonly Func<Encoding> get_output_encoding;

    public SystemConsoleCapabilityDetector()
        : this(
            () => global::System.Console.IsInputRedirected,
            () => global::System.Console.IsOutputRedirected,
            () => Environment.GetEnvironmentVariable("TERM"),
            () => Environment.GetEnvironmentVariable("NO_COLOR"),
            () => global::System.Console.OutputEncoding)
    {
    }

    public SystemConsoleCapabilityDetector(
        Func<bool> is_input_redirected,
        Func<bool> is_output_redirected,
        Func<string?> get_term,
        Func<string?> get_no_color,
        Func<Encoding> get_output_encoding)
    {
        ArgumentNullException.ThrowIfNull(is_input_redirected);
        ArgumentNullException.ThrowIfNull(is_output_redirected);
        ArgumentNullException.ThrowIfNull(get_term);
        ArgumentNullException.ThrowIfNull(get_no_color);
        ArgumentNullException.ThrowIfNull(get_output_encoding);

        this.is_input_redirected = is_input_redirected;
        this.is_output_redirected = is_output_redirected;
        this.get_term = get_term;
        this.get_no_color = get_no_color;
        this.get_output_encoding = get_output_encoding;
    }

    public ConsoleCapabilities detect(
        RuntimePlatform platform)
    {
        bool input_redirected = is_input_redirected();
        bool output_redirected = is_output_redirected();
        string term = get_term() ?? string.Empty;
        bool terminal_declares_no_capability =
            string.Equals(
                term,
                "dumb",
                StringComparison.OrdinalIgnoreCase);
        bool no_color =
            !string.IsNullOrWhiteSpace(get_no_color());
        Encoding encoding = get_output_encoding();

        bool interactive_input = !input_redirected;
        bool interactive_output =
            !output_redirected &&
            !terminal_declares_no_capability;
        bool unicode =
            supports_unicode_encoding(encoding);
        bool ansi =
            interactive_output &&
            !no_color &&
            (platform != RuntimePlatform.Windows ||
             !string.IsNullOrWhiteSpace(term) ||
             !output_redirected);
        bool console_beep =
            platform == RuntimePlatform.Windows &&
            interactive_output;
        bool terminal_bell =
            platform != RuntimePlatform.Windows &&
            interactive_output;

        return new ConsoleCapabilities(
            input_redirected,
            output_redirected,
            interactive_input,
            unicode,
            ansi,
            interactive_output,
            console_beep,
            terminal_bell);
    }

    private static bool supports_unicode_encoding(
        Encoding encoding)
    {
        return encoding.CodePage is 65001 or 1200 or 1201
            || encoding.WebName.Contains(
                "utf",
                StringComparison.OrdinalIgnoreCase);
    }
}
