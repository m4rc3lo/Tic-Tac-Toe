
namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Observa transições para detectar ciclos em cenários controlados.
/// </summary>
public interface INavigationCycleDetector
{
    void observe(
        ScreenState source,
        ScreenState target);
}
