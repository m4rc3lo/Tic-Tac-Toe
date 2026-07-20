
using TicTacToe.Persistence;

namespace TicTacToe.Composition;

internal static class PersistenceComposition
{
    public static MatchPersistenceComponents create(
        ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        string data_directory = Path.Combine(
            AppContext.BaseDirectory,
            settings.Directories.Data);
        IMatchHistoryRepository history_repository =
            new JsonMatchHistoryRepository(
                Path.Combine(data_directory, "matches.json"));
        IMatchStatisticsRepository statistics_repository =
            new JsonMatchStatisticsRepository(
                Path.Combine(data_directory, "statistics.json"));
        MatchStatisticsCalculator calculator = new();

        return new MatchPersistenceComponents(
            new MatchPersistenceService(
                history_repository,
                statistics_repository,
                new MatchRecordMapper(),
                calculator),
            new MatchPersistenceRecoveryService(
                history_repository,
                statistics_repository,
                calculator));
    }
}

internal sealed record MatchPersistenceComponents(
    IMatchPersistenceService PersistenceService,
    IMatchPersistenceRecoveryService RecoveryService);
