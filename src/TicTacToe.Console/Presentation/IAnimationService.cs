namespace TicTacToe.Presentation;

/// <summary>
/// Define animações textuais independentes do domínio e da aplicação.
/// </summary>
public interface IAnimationService
{
    void write_progressive(
        string text,
        TimeSpan character_delay,
        bool append_line = true);

    void show_analysis_indicator(
        string message,
        int frame_count,
        TimeSpan frame_delay);

    void show_progress_bar(
        int current,
        int total,
        int width = 20);
}
