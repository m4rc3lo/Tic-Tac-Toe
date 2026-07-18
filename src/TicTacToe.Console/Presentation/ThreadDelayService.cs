namespace TicTacToe.Presentation;

/// <summary>
/// Implementa atrasos reais usando a infraestrutura de execução do .NET.
/// </summary>
public sealed class ThreadDelayService : IDelayService
{
    public void wait(TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(duration),
                duration,
                "A duração não pode ser negativa.");
        }

        if (duration > TimeSpan.Zero)
        {
            Thread.Sleep(duration);
        }
    }
}
