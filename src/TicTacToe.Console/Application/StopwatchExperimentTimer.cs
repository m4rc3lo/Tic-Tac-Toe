using System.Diagnostics;

namespace TicTacToe.Application;

/// <summary>
/// Mede execuções reais com relógio monotônico.
/// </summary>
public sealed class StopwatchExperimentTimerFactory : IExperimentTimerFactory
{
    public IExperimentTimer start() => new StopwatchExperimentTimer();

    private sealed class StopwatchExperimentTimer : IExperimentTimer
    {
        private readonly Stopwatch stopwatch = Stopwatch.StartNew();

        public StopwatchExperimentTimer()
        {
            StartedAt = DateTimeOffset.UtcNow;
            FinishedAt = StartedAt;
        }

        public DateTimeOffset StartedAt { get; }
        public DateTimeOffset FinishedAt { get; private set; }
        public long ElapsedMilliseconds => stopwatch.ElapsedMilliseconds;

        public void stop()
        {
            stopwatch.Stop();
            FinishedAt = StartedAt + stopwatch.Elapsed;
        }
    }
}
