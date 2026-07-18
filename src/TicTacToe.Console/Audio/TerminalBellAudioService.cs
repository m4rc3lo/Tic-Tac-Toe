namespace TicTacToe.Audio;

/// <summary>
/// Emite o caractere de campainha do terminal.
/// </summary>
public sealed class TerminalBellAudioService : IAudioService
{
    private readonly TextWriter writer;

    /// <summary>
    /// Inicializa o serviço com o fluxo textual do terminal.
    /// </summary>
    /// <param name="writer">Fluxo que receberá o caractere BEL.</param>
    public TerminalBellAudioService(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    /// <inheritdoc />
    public void play(AudioCue cue)
    {
        writer.Write('\a');
        writer.Flush();
    }
}
