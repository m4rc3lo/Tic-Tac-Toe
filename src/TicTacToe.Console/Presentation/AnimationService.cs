namespace TicTacToe.Presentation;

/// <summary>
/// Implementa animações textuais usando um atraso injetável.
/// </summary>
public sealed class AnimationService : IAnimationService
{
    private static readonly char[] analysis_frames =
        ['|', '/', '-', '\\'];

    private readonly TextWriter writer;
    private readonly IDelayService delay_service;
    private readonly PresentationPreferences preferences;

    public AnimationService(
        TextWriter writer,
        IDelayService delay_service,
        PresentationPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(delay_service);
        ArgumentNullException.ThrowIfNull(preferences);

        this.writer = writer;
        this.delay_service = delay_service;
        this.preferences = preferences;
    }

    public void write_progressive(
        string text,
        TimeSpan character_delay,
        bool append_line = true)
    {
        ArgumentNullException.ThrowIfNull(text);
        validate_duration(character_delay);

        if (!preferences.VisualEffects)
        {
            writer.Write(text);

            if (append_line)
            {
                writer.WriteLine();
            }

            return;
        }

        foreach (char character in text)
        {
            writer.Write(character);
            delay_service.wait(character_delay);
        }

        if (append_line)
        {
            writer.WriteLine();
        }
    }

    public void show_analysis_indicator(
        string message,
        int frame_count,
        TimeSpan frame_delay)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        validate_duration(frame_delay);

        if (frame_count < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(frame_count),
                frame_count,
                "A quantidade de quadros não pode ser negativa.");
        }

        if (!preferences.VisualEffects || frame_count == 0)
        {
            writer.WriteLine(message);
            return;
        }

        for (int frame_index = 0;
             frame_index < frame_count;
             frame_index++)
        {
            char frame =
                analysis_frames[frame_index % analysis_frames.Length];

            writer.Write($"\r{message} {frame}");
            delay_service.wait(frame_delay);
        }

        writer.WriteLine();
    }

    public void show_progress_bar(
        int current,
        int total,
        int width = 20)
    {
        if (total <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(total),
                total,
                "O total deve ser maior que zero.");
        }

        if (current < 0 || current > total)
        {
            throw new ArgumentOutOfRangeException(
                nameof(current),
                current,
                "O valor atual deve pertencer ao intervalo do progresso.");
        }

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(width),
                width,
                "A largura deve ser maior que zero.");
        }

        int completed_width =
            (int)Math.Round(
                (double)current / total * width,
                MidpointRounding.AwayFromZero);

        string completed = new('#', completed_width);
        string pending = new('-', width - completed_width);
        int percentage =
            (int)Math.Round(
                (double)current / total * 100,
                MidpointRounding.AwayFromZero);

        writer.WriteLine(
            $"[{completed}{pending}] {percentage}%");
    }

    private static void validate_duration(TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(duration),
                duration,
                "A duração não pode ser negativa.");
        }
    }
}
