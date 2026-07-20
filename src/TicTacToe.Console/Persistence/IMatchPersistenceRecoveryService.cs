
namespace TicTacToe.Persistence;

/// <summary>
/// Define a recuperação da consistência entre histórico e estatísticas.
/// </summary>
public interface IMatchPersistenceRecoveryService
{
    MatchStatisticsRecord recover();
}
