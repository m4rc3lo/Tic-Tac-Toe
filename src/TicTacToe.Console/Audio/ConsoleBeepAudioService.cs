namespace TicTacToe.Audio;

/// <summary>
/// Reproduz sinais por <see cref="Console.Beep(int, int)"/>.
/// </summary>
/// <remarks>
/// Esta implementação é destinada prioritariamente ao Windows. O suporte
/// efetivo depende do terminal, do sistema e do dispositivo de áudio.
/// </remarks>
public sealed class ConsoleBeepAudioService : IAudioService
{
    /// <inheritdoc />
    public void play(AudioCue cue)
    {
        (int frequency, int duration) = cue switch
        {
            AudioCue.Move => (660, 70),
            AudioCue.InvalidMove => (220, 160),
            AudioCue.Victory => (880, 220),
            AudioCue.Defeat => (196, 260),
            AudioCue.Draw => (440, 180),
            AudioCue.Menu => (520, 60),
            _ => throw new ArgumentOutOfRangeException(
                nameof(cue),
                cue,
                "O evento sonoro não é reconhecido.")
        };

        Console.Beep(frequency, duration);
    }
}
