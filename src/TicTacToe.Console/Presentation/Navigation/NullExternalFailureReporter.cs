
namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Ignora mensagens quando não há interface configurada.
/// </summary>
public sealed class NullExternalFailureReporter
    : IExternalFailureReporter
{
    public void report(
        string context,
        Exception exception)
    {
    }
}
