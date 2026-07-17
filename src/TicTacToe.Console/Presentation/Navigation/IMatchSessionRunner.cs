using TicTacToe.Domain;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Define a execução de uma sessão de partida solicitada pela apresentação.
/// </summary>
public interface IMatchSessionRunner
{
    /// <summary>
    /// Executa uma partida conforme a configuração selecionada.
    /// </summary>
    /// <param name="configuration">Configuração da sessão.</param>
    /// <returns>Partida resultante.</returns>
    Match play(MatchConfiguration configuration);
}
