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

    [Fact]
    public void play_should_not_hide_unexpected_programming_failure()
    {
        FallbackAudioService service = new(
            new UnexpectedFailureAudioService(),
            new RecordingAudioService());

        Assert.Throws<InvalidOperationException>(
            () => service.play(AudioCue.Move));
    }

    private sealed class ThrowingAudioService : IAudioService
    {
        public int CallCount { get; private set; }

        public void play(AudioCue cue)
        {
            CallCount++;
            throw new IOException(
                "Dispositivo indisponível.");
        }
    }

    private sealed class UnexpectedFailureAudioService
        : IAudioService
    {
        public void play(AudioCue cue)
        {
            throw new InvalidOperationException(
                "Falha de programação.");
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
