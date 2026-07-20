
namespace TicTacToe.Audio;

/// <summary>
/// Protege a aplicação contra falhas operacionais do áudio.
/// </summary>
public sealed class FallbackAudioService : IAudioService
{
    private readonly IAudioService fallback_service;
    private IAudioService active_service;

    public FallbackAudioService(
        IAudioService primary_service,
        IAudioService fallback_service)
    {
        ArgumentNullException.ThrowIfNull(primary_service);
        ArgumentNullException.ThrowIfNull(fallback_service);

        active_service = primary_service;
        this.fallback_service = fallback_service;
    }

    public void play(AudioCue cue)
    {
        try
        {
            active_service.play(cue);
        }
        catch (Exception exception)
            when (is_expected_audio_failure(exception))
        {
            active_service = fallback_service;

            try
            {
                fallback_service.play(cue);
            }
            catch (Exception fallback_exception)
                when (is_expected_audio_failure(fallback_exception))
            {
            }
        }
    }

    private static bool is_expected_audio_failure(
        Exception exception)
    {
        return exception is IOException
            or UnauthorizedAccessException
            or PlatformNotSupportedException
            or ObjectDisposedException;
    }
}
