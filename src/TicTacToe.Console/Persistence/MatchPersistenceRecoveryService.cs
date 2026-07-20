
namespace TicTacToe.Persistence;

/// <summary>
/// Recalcula estatísticas a partir da fonte autoritativa de partidas.
/// </summary>
public sealed class MatchPersistenceRecoveryService
    : IMatchPersistenceRecoveryService
{
    private readonly IMatchHistoryRepository history_repository;
    private readonly IMatchStatisticsRepository statistics_repository;
    private readonly MatchStatisticsCalculator calculator;

    public MatchPersistenceRecoveryService(
        IMatchHistoryRepository history_repository,
        IMatchStatisticsRepository statistics_repository,
        MatchStatisticsCalculator calculator)
    {
        ArgumentNullException.ThrowIfNull(history_repository);
        ArgumentNullException.ThrowIfNull(statistics_repository);
        ArgumentNullException.ThrowIfNull(calculator);

        this.history_repository = history_repository;
        this.statistics_repository = statistics_repository;
        this.calculator = calculator;
    }

    public MatchStatisticsRecord recover()
    {
        IReadOnlyList<MatchRecord> history =
            history_repository.load_all();
        MatchStatisticsRecord statistics =
            calculator.calculate(history);

        statistics_repository.save(statistics);
        return statistics;
    }
}
