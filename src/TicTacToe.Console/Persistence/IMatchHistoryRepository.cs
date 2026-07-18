namespace TicTacToe.Persistence;

/// <summary>
/// Define persistência do histórico de partidas.
/// </summary>
public interface IMatchHistoryRepository
{
    IReadOnlyList<MatchRecord> load_all();

    void replace_all(IReadOnlyList<MatchRecord> matches);
}
