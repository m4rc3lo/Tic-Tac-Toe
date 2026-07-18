namespace TicTacToe.Audio;

/// <summary>
/// Define a reprodução de sinais sonoros da aplicação.
/// </summary>
public interface IAudioService
{
    /// <summary>
    /// Reproduz o sinal associado ao evento informado.
    /// </summary>
    /// <param name="cue">Evento sonoro solicitado.</param>
    void play(AudioCue cue);
}
