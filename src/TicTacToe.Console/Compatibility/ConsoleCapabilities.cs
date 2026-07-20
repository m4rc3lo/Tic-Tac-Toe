
namespace TicTacToe.Compatibility;

/// <summary>
/// Descreve capacidades observadas no ambiente Console atual.
/// </summary>
public sealed record ConsoleCapabilities(
    bool IsInputRedirected,
    bool IsOutputRedirected,
    bool SupportsInteractiveInput,
    bool SupportsUnicode,
    bool SupportsAnsi,
    bool SupportsClearScreen,
    bool SupportsConsoleBeep,
    bool SupportsTerminalBell);
