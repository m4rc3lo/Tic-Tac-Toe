using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public class AnimationServiceTests
{
    [Fact]
    public void write_progressive_should_write_text_without_real_wait()
    {
        StringWriter writer = new();
        RecordingDelayService delay = new();
        AnimationService service = new(
            writer,
            delay,
            new PresentationPreferences(
                visual_effects: true));

        service.write_progressive(
            "ABC",
            TimeSpan.FromMilliseconds(1));

        Assert.Equal(
            "ABC" + Environment.NewLine,
            writer.ToString());
        Assert.Equal(3, delay.Calls.Count);
    }

    [Fact]
    public void show_analysis_indicator_should_write_requested_frames()
    {
        StringWriter writer = new();
        RecordingDelayService delay = new();
        AnimationService service = new(
            writer,
            delay,
            new PresentationPreferences(
                visual_effects: true));

        service.show_analysis_indicator(
            "Analisando",
            frame_count: 4,
            frame_delay: TimeSpan.FromMilliseconds(1));

        string text = writer.ToString();

        Assert.Contains("Analisando |", text);
        Assert.Contains("Analisando /", text);
        Assert.Contains("Analisando -", text);
        Assert.Contains("Analisando \\", text);
        Assert.Equal(4, delay.Calls.Count);
    }

    [Fact]
    public void show_progress_bar_should_write_exact_progress()
    {
        StringWriter writer = new();
        AnimationService service = new(
            writer,
            new ImmediateDelayService(),
            new PresentationPreferences());

        service.show_progress_bar(
            current: 3,
            total: 4,
            width: 8);

        Assert.Equal(
            "[######--] 75%" + Environment.NewLine,
            writer.ToString());
    }

    private sealed class RecordingDelayService : IDelayService
    {
        public List<TimeSpan> Calls { get; } = [];

        public void wait(TimeSpan duration)
        {
            Calls.Add(duration);
        }
    }
}
