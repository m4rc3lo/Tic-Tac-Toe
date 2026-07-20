
namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Limita transições somente quando explicitamente configurado.
/// </summary>
public sealed class TransitionLimitCycleDetector
    : INavigationCycleDetector
{
    private readonly int max_transitions;
    private int transition_count;

    public TransitionLimitCycleDetector(int max_transitions)
    {
        if (max_transitions <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(max_transitions),
                max_transitions,
                "O limite de transições deve ser maior que zero.");
        }

        this.max_transitions = max_transitions;
    }

    public void observe(
        ScreenState source,
        ScreenState target)
    {
        transition_count++;

        if (transition_count > max_transitions)
        {
            throw new InvalidOperationException(
                "O limite de transições de teste foi excedido. " +
                "A navegação simulada pode conter um ciclo.");
        }
    }
}
