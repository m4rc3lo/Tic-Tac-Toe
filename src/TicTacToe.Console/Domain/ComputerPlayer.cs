using TicTacToe.AI;

namespace TicTacToe.Domain;

/// <summary>
/// Representa um participante controlado por uma estratégia de jogada.
/// </summary>
/// <remarks>
/// A classe atua como contexto do padrão Strategy: mantém uma implementação de
/// <see cref="IMoveStrategy"/> e delega a ela a seleção de posições.
/// </remarks>
public sealed class ComputerPlayer : Player
{
    /// <summary>
    /// Inicializa um participante computacional.
    /// </summary>
    /// <param name="name">Nome de exibição.</param>
    /// <param name="symbol">Símbolo controlado.</param>
    /// <param name="strategy">Estratégia utilizada para escolher jogadas.</param>
    public ComputerPlayer(
        string name,
        Symbol symbol,
        IMoveStrategy strategy)
        : base(name, symbol)
    {
        ArgumentNullException.ThrowIfNull(strategy);
        Strategy = strategy;
    }

    /// <summary>
    /// Obtém a estratégia associada ao participante.
    /// </summary>
    public IMoveStrategy Strategy { get; }

    /// <summary>
    /// Solicita à estratégia uma posição válida.
    /// </summary>
    /// <param name="board">Tabuleiro atual.</param>
    /// <returns>Posição selecionada pela estratégia.</returns>
    public BoardPosition choose_move(Board board)
    {
        return Strategy.choose_move(board, Symbol);
    }
}
