using TicTacToe.Audio;
using TicTacToe.Compatibility;
using Xunit;

namespace TicTacToe.Tests.Audio;

public class AudioServiceSelectorTests
{
    [Fact]
    public void select_should_return_silent_service_when_disabled()
    {
        RecordingAudioService windows = new();
        RecordingAudioService terminal = new();
        SilentAudioService silent = new();

        AudioServiceSelector selector = new(
            () => true,
            () => windows,
            () => terminal,
            () => silent);

        IAudioService selected =
            selector.select(audio_enabled: false);

        Assert.Same(silent, selected);
    }

    [Fact]
    public void select_should_use_windows_service_on_windows()
    {
        RecordingAudioService windows = new();

        AudioServiceSelector selector = new(
            () => true,
            () => windows,
            () => new RecordingAudioService(),
            () => new SilentAudioService());

        IAudioService selected =
            selector.select(audio_enabled: true);

        selected.play(AudioCue.Menu);

        Assert.Equal([AudioCue.Menu], windows.Cues);
    }

    [Fact]
    public void select_should_use_terminal_service_outside_windows()
    {
        RecordingAudioService terminal = new();

        AudioServiceSelector selector = new(
            () => false,
            () => new RecordingAudioService(),
            () => terminal,
            () => new SilentAudioService());

        IAudioService selected =
            selector.select(audio_enabled: true);

        selected.play(AudioCue.Move);

        Assert.Equal([AudioCue.Move], terminal.Cues);
    }


    [Fact]
    public void select_should_use_silent_service_when_platform_lacks_audio_capability()
    {
        RecordingAudioService windows = new();
        RecordingAudioService terminal = new();
        SilentAudioService silent = new();
        AudioServiceSelector selector = new(
            () => true,
            () => windows,
            () => terminal,
            () => silent);
        ConsoleCapabilities capabilities = new(
            false,
            true,
            true,
            true,
            false,
            false,
            false,
            false);

        IAudioService selected = selector.select(
            audio_enabled: true,
            RuntimePlatform.Windows,
            capabilities);

        Assert.Same(silent, selected);
    }

    [Fact]
    public void select_should_use_terminal_bell_by_capability_on_other_platform()
    {
        RecordingAudioService terminal = new();
        AudioServiceSelector selector = new(
            () => false,
            () => new RecordingAudioService(),
            () => terminal,
            () => new SilentAudioService());
        ConsoleCapabilities capabilities = new(
            false,
            false,
            true,
            true,
            true,
            true,
            false,
            true);

        IAudioService selected = selector.select(
            audio_enabled: true,
            RuntimePlatform.Other,
            capabilities);

        selected.play(AudioCue.Move);

        Assert.Equal([AudioCue.Move], terminal.Cues);
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
