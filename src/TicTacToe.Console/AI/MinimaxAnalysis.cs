using TicTacToe.Domain;

namespace TicTacToe.AI;

/// <summary>
/// Representa o resultado imutável de uma análise Minimax.
/// </summary>
/// <param name="Position">Melhor posição encontrada.</param>
/// <param name="Score">Pontuação da posição para o agente analisado.</param>
/// <param name="MaximumDepth">Maior profundidade alcançada pela busca.</param>
/// <param name="VisitedStates">Quantidade de estados visitados.</param>
/// <remarks>
/// O resultado é separado de <see cref="Move"/> porque descreve uma decisão
/// hipotética, ainda não aplicada pelo agregado da partida.
/// </remarks>
public readonly record struct MinimaxAnalysis(
    BoardPosition Position,
    int Score,
    int MaximumDepth,
    int VisitedStates);
