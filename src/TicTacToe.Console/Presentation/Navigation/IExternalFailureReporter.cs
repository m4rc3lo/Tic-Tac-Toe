
namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Apresenta falhas externas sem expor dados sensíveis.
/// </summary>
public interface IExternalFailureReporter
{
    void report(
        string context,
        Exception exception);
}
