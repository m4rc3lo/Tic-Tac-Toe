namespace TicTacToe.Domain;

/// <summary>
/// Representa um participante controlado computacionalmente.
/// </summary>
/// <remarks>
/// A estratégia de decisão será associada em uma etapa posterior. Nesta versão,
/// a classe identifica apenas a natureza do participante no domínio.
/// </remarks>
public sealed class ComputerPlayer : Player
{
    /// <summary>
    /// Inicializa um participante computacional.
    /// </summary>
    /// <param name="name">Nome de exibição.</param>
    /// <param name="symbol">Símbolo controlado.</param>
    public ComputerPlayer(string name, Symbol symbol)
        : base(name, symbol)
    {
    }
}
