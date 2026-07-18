namespace TicTacToe.Audio;

/// <summary>
/// Seleciona uma implementação de áudio conforme configuração e plataforma.
/// </summary>
public sealed class AudioServiceSelector
{
    private readonly Func<bool> is_windows;
    private readonly Func<IAudioService> windows_factory;
    private readonly Func<IAudioService> terminal_factory;
    private readonly Func<IAudioService> silent_factory;

    /// <summary>
    /// Inicializa o seletor para uso na aplicação.
    /// </summary>
    /// <param name="terminal_writer">Fluxo usado pelo terminal bell.</param>
    public AudioServiceSelector(TextWriter terminal_writer)
        : this(
            OperatingSystem.IsWindows,
            () => new ConsoleBeepAudioService(),
            () => new TerminalBellAudioService(terminal_writer),
            () => new SilentAudioService())
    {
        ArgumentNullException.ThrowIfNull(terminal_writer);
    }

    /// <summary>
    /// Inicializa o seletor com fábricas injetáveis para testes.
    /// </summary>
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

    /// <summary>
    /// Seleciona um serviço protegido por fallback silencioso.
    /// </summary>
    /// <param name="audio_enabled">Indica se o áudio está habilitado.</param>
    /// <returns>Serviço apropriado à configuração.</returns>
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
}
