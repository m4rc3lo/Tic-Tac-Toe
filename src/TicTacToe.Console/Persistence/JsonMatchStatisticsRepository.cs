namespace TicTacToe.Persistence;

/// <summary>
/// Persiste estatísticas agregadas em JSON.
/// </summary>
public sealed class JsonMatchStatisticsRepository
    : IMatchStatisticsRepository
{
    private readonly string path;
    private readonly JsonFileStore store = new();

    public JsonMatchStatisticsRepository(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        this.path = path;
    }

    public MatchStatisticsRecord load()
    {
        return store.load_or_default(
            path,
            MatchStatisticsRecord.empty());
    }

    public void save(MatchStatisticsRecord statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        store.save(path, statistics);
    }
}
