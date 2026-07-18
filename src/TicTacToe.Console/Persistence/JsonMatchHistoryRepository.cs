namespace TicTacToe.Persistence;

/// <summary>
/// Persiste o histórico completo de partidas em JSON.
/// </summary>
public sealed class JsonMatchHistoryRepository
    : IMatchHistoryRepository
{
    private readonly string path;
    private readonly JsonFileStore store = new();

    public JsonMatchHistoryRepository(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        this.path = path;
    }

    public IReadOnlyList<MatchRecord> load_all()
    {
        MatchRecord[] records =
            store.load_or_default(
                path,
                Array.Empty<MatchRecord>());

        return Array.AsReadOnly(records);
    }

    public void replace_all(
        IReadOnlyList<MatchRecord> matches)
    {
        ArgumentNullException.ThrowIfNull(matches);
        store.save(path, matches);
    }
}
