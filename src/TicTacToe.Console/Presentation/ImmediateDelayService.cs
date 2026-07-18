namespace TicTacToe.Presentation;

/// <summary>
/// Ignora atrasos, permitindo testes e execução sem espera real.
/// </summary>
public sealed class ImmediateDelayService : IDelayService
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
    }
}
