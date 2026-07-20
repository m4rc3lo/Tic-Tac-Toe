
using TicTacToe.Compatibility;

namespace TicTacToe.Audio;

/// <summary>
/// Seleciona áudio conforme configuração, plataforma e capacidade.
/// </summary>
public sealed class AudioServiceSelector
{
    private readonly Func<bool> is_windows;
    private readonly Func<IAudioService> windows_factory;
    private readonly Func<IAudioService> terminal_factory;
    private readonly Func<IAudioService> silent_factory;

    public AudioServiceSelector(TextWriter terminal_writer)
        : this(
            OperatingSystem.IsWindows,
            () => new ConsoleBeepAudioService(),
            () => new TerminalBellAudioService(terminal_writer),
            () => new SilentAudioService())
    {
        ArgumentNullException.ThrowIfNull(terminal_writer);
    }

    public AudioServiceSelector(
        Func<bool> is_windows,
        Func<IAudioService> windows_factory,
        Func<IAudioService> terminal_factory,
        Func<IAudioService> silent_factory)
    {
        ArgumentNullException.ThrowIfNull(is_windows);
        ArgumentNullException.ThrowIfNull(windows_factory);
        ArgumentNullException.ThrowIfNull(terminal_factory);
        ArgumentNullException.ThrowIfNull(silent_factory);

        this.is_windows = is_windows;
        this.windows_factory = windows_factory;
        this.terminal_factory = terminal_factory;
        this.silent_factory = silent_factory;
    }

    public IAudioService select(bool audio_enabled)
    {
        IAudioService silent_service = silent_factory();

        if (!audio_enabled)
        {
            return silent_service;
        }

        IAudioService primary_service = is_windows()
            ? windows_factory()
            : terminal_factory();

        return new FallbackAudioService(
            primary_service,
            silent_service);
    }

    public IAudioService select(
        bool audio_enabled,
        RuntimePlatform platform,
        ConsoleCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);

        IAudioService silent_service = silent_factory();

        if (!audio_enabled)
        {
            return silent_service;
        }

        IAudioService? primary_service = platform switch
        {
            RuntimePlatform.Windows
                when capabilities.SupportsConsoleBeep =>
                    windows_factory(),
            RuntimePlatform.Linux or RuntimePlatform.MacOS
                when capabilities.SupportsTerminalBell =>
                    terminal_factory(),
            RuntimePlatform.Other
                when capabilities.SupportsTerminalBell =>
                    terminal_factory(),
            _ => null
        };

        return primary_service is null
            ? silent_service
            : new FallbackAudioService(
                primary_service,
                silent_service);
    }
}
