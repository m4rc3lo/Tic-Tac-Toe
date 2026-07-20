
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

        try
        {
            history_repository.replace_all(updated_history);
            statistics_repository.save(
                calculator.calculate(updated_history));
            return record;
        }
        catch (Exception exception)
            when (is_infrastructure_failure(exception))
        {
            try
            {
                history_repository.replace_all(previous_history);
            }
            catch (Exception rollback_exception)
                when (is_infrastructure_failure(rollback_exception))
            {
                throw new InfrastructureOperationException(
                    "persistir partida e restaurar histórico",
                    "Não foi possível salvar a partida nem restaurar integralmente os dados anteriores.",
                    new AggregateException(
                        exception,
                        rollback_exception));
            }

            throw new InfrastructureOperationException(
                "persistir partida",
                "A partida terminou, mas não foi possível salvá-la.",
                exception);
        }
    }

    private static bool is_infrastructure_failure(
        Exception exception)
    {
        return exception is InfrastructureOperationException
            or IOException
            or UnauthorizedAccessException;
    }
}
