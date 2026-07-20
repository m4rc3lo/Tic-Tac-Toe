
namespace TicTacToe.Application;

/// <summary>
/// Ignora notificações quando não há canal diagnóstico.
/// </summary>
public sealed class NullExperimentInfrastructureReporter
    : IExperimentInfrastructureReporter
{
    public void report(
        string repository_name,
        Exception exception)
    {
    }
}
