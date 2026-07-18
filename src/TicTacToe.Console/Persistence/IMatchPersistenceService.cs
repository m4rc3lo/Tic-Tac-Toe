namespace TicTacToe.Persistence;

/// <summary>
/// Coordena o salvamento da partida e a atualização das estatísticas.
/// </summary>
public interface IMatchPersistenceService
{
    MatchRecord persist(MatchPersistenceContext context);
}
