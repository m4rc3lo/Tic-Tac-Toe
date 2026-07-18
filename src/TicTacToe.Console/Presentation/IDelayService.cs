namespace TicTacToe.Presentation;

/// <summary>
/// Abstrai a espera usada por animações da camada de apresentação.
/// </summary>
public interface IDelayService
{
    /// <summary>
    /// Aguarda o intervalo informado.
    /// </summary>
    void wait(TimeSpan duration);
}
