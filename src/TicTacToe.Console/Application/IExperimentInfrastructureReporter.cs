
namespace TicTacToe.Application;

/// <summary>
/// Recebe falhas de infraestrutura da persistência experimental.
/// </summary>
public interface IExperimentInfrastructureReporter
{
    void report(
        string repository_name,
        Exception exception);
}
