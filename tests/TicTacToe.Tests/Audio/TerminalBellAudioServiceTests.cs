using TicTacToe.Audio;
using Xunit;

namespace TicTacToe.Tests.Audio;

public class TerminalBellAudioServiceTests
{
    [Fact]
    public void play_should_write_terminal_bell_without_audio_device()
    {
        StringWriter writer = new();
        TerminalBellAudioService service = new(writer);

        service.play(AudioCue.Move);

        Assert.Equal("\a", writer.ToString());
    }

    [Fact]
    public void silent_service_should_not_produce_output_or_failure()
    {
        SilentAudioService service = new();

        Exception? exception = Record.Exception(
            () => service.play(AudioCue.Defeat));

        Assert.Null(exception);
    }
}
