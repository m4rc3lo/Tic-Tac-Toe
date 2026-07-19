namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Define a execução de uma partida demonstrativa IA contra IA.
/// </summary>
public interface IAutomaticMatchRunner
{
    /// <summary>
    /// Executa a demonstração configurada.
    /// </summary>
    AutomaticMatchResult run(
        AutomaticMatchConfiguration configuration);
}
