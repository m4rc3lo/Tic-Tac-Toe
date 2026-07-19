namespace TicTacToe.Application;

/// <summary>
/// Mede uma execução sem impor relógio real aos testes.
/// </summary>
public interface IExperimentTimer
{
    DateTimeOffset StartedAt { get; }
    DateTimeOffset FinishedAt { get; }
    long ElapsedMilliseconds { get; }
    void stop();
}

public interface IExperimentTimerFactory
{
    IExperimentTimer start();
}
