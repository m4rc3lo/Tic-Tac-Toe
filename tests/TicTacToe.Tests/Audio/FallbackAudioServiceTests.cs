using TicTacToe.Audio;
using Xunit;

namespace TicTacToe.Tests.Audio;

public class FallbackAudioServiceTests
{
    [Fact]
    public void play_should_use_fallback_after_primary_failure()
    {
        ThrowingAudioService primary = new();
        RecordingAudioService fallback = new();
        FallbackAudioService service = new(
            primary,
            fallback);

        service.play(AudioCue.Victory);
        service.play(AudioCue.Move);

        Assert.Equal(1, primary.CallCount);
        Assert.Equal(
            [AudioCue.Victory, AudioCue.Move],
            fallback.Cues);
    }

    [Fact]
    public void play_should_ignore_recoverable_fallback_failure()
    {
        FallbackAudioService service = new(
            new ThrowingAudioService(),
            new ThrowingAudioService());

        Exception? exception = Record.Exception(
            () => service.play(AudioCue.InvalidMove));

        Assert.Null(exception);
    }

    private sealed class ThrowingAudioService : IAudioService
    {
        public int CallCount { get; private set; }

        public void play(AudioCue cue)
        {
            CallCount++;
            throw new InvalidOperationException(
                "Dispositivo indisponível.");
        }
    }

    private sealed class RecordingAudioService : IAudioService
    {
        public List<AudioCue> Cues { get; } = [];

        public void play(AudioCue cue)
        {
            Cues.Add(cue);
        }
    }
}
