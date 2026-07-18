namespace TicTacToe.Persistence;

/// <summary>
/// Salva partidas concluídas e mantém estatísticas consistentes.
/// </summary>
public sealed class MatchPersistenceService
    : IMatchPersistenceService
{
    private readonly IMatchHistoryRepository history_repository;
    private readonly IMatchStatisticsRepository statistics_repository;
    private readonly MatchRecordMapper mapper;
    private readonly MatchStatisticsCalculator calculator;

    public MatchPersistenceService(
        IMatchHistoryRepository history_repository,
        IMatchStatisticsRepository statistics_repository,
        MatchRecordMapper mapper,
        MatchStatisticsCalculator calculator)
    {
        ArgumentNullException.ThrowIfNull(history_repository);
        ArgumentNullException.ThrowIfNull(statistics_repository);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(calculator);

        this.history_repository = history_repository;
        this.statistics_repository = statistics_repository;
        this.mapper = mapper;
        this.calculator = calculator;
    }

    public MatchRecord persist(
        MatchPersistenceContext context)
    {
        MatchRecord record = mapper.map(context);
        IReadOnlyList<MatchRecord> previous_history =
            history_repository.load_all();
        MatchRecord[] updated_history =
            [.. previous_history, record];

        history_repository.replace_all(updated_history);

        try
        {
            MatchStatisticsRecord statistics =
                calculator.calculate(updated_history);

            statistics_repository.save(statistics);
            return record;
        }
        catch
        {
            history_repository.replace_all(previous_history);
            throw;
        }
    }
}
