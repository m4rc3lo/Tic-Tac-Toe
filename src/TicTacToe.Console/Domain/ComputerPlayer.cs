namespace TicTacToe.Domain;

/// <summary>
/// Representa um participante controlado computacionalmente.
/// </summary>
/// <remarks>
/// A associação entre participante e estratégia pertence à camada de aplicação,
/// preservando o domínio contra dependências da infraestrutura de inteligência
/// artificial.
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
