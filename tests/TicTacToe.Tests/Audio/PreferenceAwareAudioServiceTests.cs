using TicTacToe.Audio;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Audio;

public class PreferenceAwareAudioServiceTests
{
    [Fact]
    public void play_should_observe_current_preference()
    {
        PresentationPreferences preferences = new(
            audio_enabled: false);
        RecordingAudioService inner = new();
        PreferenceAwareAudioService service = new(
            preferences,
            inner);

        service.play(AudioCue.Move);
        preferences.AudioEnabled = true;
        service.play(AudioCue.Victory);

        Assert.Equal([AudioCue.Victory], inner.Cues);
    }

    private sealed class RecordingAudioService : IAudioService
    {
        public List<AudioCue> Cues { get; } = [];
        public void play(AudioCue cue) => Cues.Add(cue);
    }
}
