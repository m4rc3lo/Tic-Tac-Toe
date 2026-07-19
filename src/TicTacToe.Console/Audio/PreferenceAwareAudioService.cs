using TicTacToe.Presentation;

namespace TicTacToe.Audio;

/// <summary>
/// Observa as preferências atuais antes de reproduzir cada sinal sonoro.
/// </summary>
public sealed class PreferenceAwareAudioService : IAudioService
{
    private readonly PresentationPreferences preferences;
    private readonly IAudioService enabled_service;

    /// <summary>
    /// Inicializa o serviço reativo.
    /// </summary>
    public PreferenceAwareAudioService(
        PresentationPreferences preferences,
        IAudioService enabled_service)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        ArgumentNullException.ThrowIfNull(enabled_service);

        this.preferences = preferences;
        this.enabled_service = enabled_service;
    }

    /// <inheritdoc />
    public void play(AudioCue cue)
    {
        if (preferences.AudioEnabled)
        {
            enabled_service.play(cue);
        }
    }
}
