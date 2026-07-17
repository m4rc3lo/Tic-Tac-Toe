namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Define uma tela executável pela máquina de estados de apresentação.
/// </summary>
public interface IScreen
{
    /// <summary>
    /// Obtém o estado representado pela tela.
    /// </summary>
    ScreenState State { get; }

    /// <summary>
    /// Executa a tela e retorna a próxima transição.
    /// </summary>
    /// <param name="context">Estado compartilhado da navegação.</param>
    /// <returns>Próximo estado solicitado.</returns>
    ScreenTransition show(ScreenContext context);
}
