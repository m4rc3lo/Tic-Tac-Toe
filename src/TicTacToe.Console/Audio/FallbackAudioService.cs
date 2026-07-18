namespace TicTacToe.Audio;

/// <summary>
/// Protege a aplicação contra falhas da implementação primária de áudio.
/// </summary>
public sealed class FallbackAudioService : IAudioService
{
    private readonly IAudioService fallback_service;
    private IAudioService active_service;

    /// <summary>
    /// Inicializa o serviço resiliente.
    /// </summary>
    /// <param name="primary_service">Implementação preferencial.</param>
    /// <param name="fallback_service">Implementação usada após falha.</param>
    public FallbackAudioService(
        IAudioService primary_service,
        IAudioService fallback_service)
    {
        ArgumentNullException.ThrowIfNull(primary_service);
        ArgumentNullException.ThrowIfNull(fallback_service);

        active_service = primary_service;
        this.fallback_service = fallback_service;
    }

    /// <inheritdoc />
    public void play(AudioCue cue)
    {
        try
        {
            active_service.play(cue);
        }
        catch (Exception)
        {
            active_service = fallback_service;

            try
            {
                fallback_service.play(cue);
            }
            catch (Exception)
            {
                // Áudio é recurso opcional e nunca encerra a aplicação.
            }
        }
    }
}
