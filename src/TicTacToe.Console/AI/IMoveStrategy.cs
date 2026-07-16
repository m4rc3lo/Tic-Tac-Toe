using TicTacToe.Domain;

namespace TicTacToe.AI;

/// <summary>
/// Define o contrato do padrão Strategy para seleção de jogadas.
/// </summary>
/// <remarks>
/// Implementações recebem apenas o estado necessário para escolher uma posição.
/// Elas não aplicam a jogada, não alteram o tabuleiro e não controlam o fluxo da
/// partida.
/// </remarks>
public interface IMoveStrategy
{
    /// <summary>
    /// Seleciona uma posição válida para o símbolo informado.
    /// </summary>
    /// <param name="board">Tabuleiro consultado.</param>
    /// <param name="symbol">Símbolo controlado pelo agente.</param>
    /// <returns>Posição escolhida entre as casas disponíveis.</returns>
    BoardPosition choose_move(Board board, Symbol symbol);
}
