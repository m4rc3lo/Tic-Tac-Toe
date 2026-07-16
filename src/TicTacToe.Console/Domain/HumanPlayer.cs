namespace TicTacToe.Domain;

/// <summary>
/// Representa uma pessoa participante.
/// </summary>
public sealed class HumanPlayer : Player
{
    /// <summary>
    /// Inicializa uma pessoa participante.
    /// </summary>
    /// <param name="name">Nome de exibição.</param>
    /// <param name="symbol">Símbolo controlado.</param>
    public HumanPlayer(string name, Symbol symbol)
        : base(name, symbol)
    {
    }
}
