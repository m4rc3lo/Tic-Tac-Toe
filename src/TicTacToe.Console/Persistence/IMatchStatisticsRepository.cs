namespace TicTacToe.Persistence;

/// <summary>
/// Define persistência das estatísticas agregadas.
/// </summary>
public interface IMatchStatisticsRepository
{
    MatchStatisticsRecord load();

    void save(MatchStatisticsRecord statistics);
}
