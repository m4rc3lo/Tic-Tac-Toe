namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Abstrai pausa e cancelamento do modo automático.
/// </summary>
public interface IAutomaticModeControl
{
    /// <summary>
    /// Aguarda autorização para executar o próximo turno.
    /// </summary>
    AutomaticControlDecision wait_for_turn();
}
