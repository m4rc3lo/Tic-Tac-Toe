namespace TicTacToe.Audio;

/// <summary>
/// Implementa áudio nulo para execução silenciosa e fallback.
/// </summary>
public sealed class SilentAudioService : IAudioService
{
    /// <inheritdoc />
    public void play(AudioCue cue)
    {
    }
}
