namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Representa a transição solicitada por uma tela.
/// </summary>
/// <param name="Target">Próximo estado de navegação.</param>
public readonly record struct ScreenTransition(ScreenState Target);
