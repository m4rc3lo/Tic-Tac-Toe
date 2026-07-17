using TicTacToe.Domain;

namespace TicTacToe.AI;

/// <summary>
/// Representa um estado mutável restrito à busca de inteligência artificial.
/// </summary>
/// <remarks>
/// A instância copia os nove símbolos de um <see cref="IReadOnlyBoard"/>. Todas
/// as aplicações e reversões realizadas pelo Minimax ocorrem somente nesta
/// representação, preservando o tabuleiro pertencente a <see cref="Match"/>.
/// </remarks>
internal sealed class SearchBoard
{
    private static readonly BoardPosition[][] winning_lines =
    [
        [new BoardPosition(0, 0), new BoardPosition(0, 1), new BoardPosition(0, 2)],
        [new BoardPosition(1, 0), new BoardPosition(1, 1), new BoardPosition(1, 2)],
        [new BoardPosition(2, 0), new BoardPosition(2, 1), new BoardPosition(2, 2)],
        [new BoardPosition(0, 0), new BoardPosition(1, 0), new BoardPosition(2, 0)],
        [new BoardPosition(0, 1), new BoardPosition(1, 1), new BoardPosition(2, 1)],
        [new BoardPosition(0, 2), new BoardPosition(1, 2), new BoardPosition(2, 2)],
        [new BoardPosition(0, 0), new BoardPosition(1, 1), new BoardPosition(2, 2)],
        [new BoardPosition(0, 2), new BoardPosition(1, 1), new BoardPosition(2, 0)]
    ];

    private readonly Symbol[,] cells =
        new Symbol[BoardPosition.BoardSize, BoardPosition.BoardSize];

    /// <summary>
    /// Inicializa o estado de busca copiando um tabuleiro somente para leitura.
    /// </summary>
    /// <param name="board">Tabuleiro de origem.</param>
    public SearchBoard(IReadOnlyBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                BoardPosition position = new(row, column);
                cells[row, column] = board.get_symbol(position);
            }
        }
    }

    /// <summary>
    /// Obtém as posições livres em ordem estável de linha e coluna.
    /// </summary>
    /// <returns>Posições disponíveis.</returns>
    public IReadOnlyList<BoardPosition> get_available_positions()
    {
        List<BoardPosition> positions = [];

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                BoardPosition position = new(row, column);

                if (get_symbol(position) == Symbol.Empty)
                {
                    positions.Add(position);
                }
            }
        }

        return positions.AsReadOnly();
    }

    /// <summary>
    /// Obtém o símbolo de uma posição.
    /// </summary>
    /// <param name="position">Posição consultada.</param>
    /// <returns>Símbolo armazenado.</returns>
    public Symbol get_symbol(BoardPosition position)
    {
        return cells[position.Row, position.Column];
    }

    /// <summary>
    /// Define temporariamente o símbolo de uma posição.
    /// </summary>
    /// <param name="position">Posição alterada.</param>
    /// <param name="symbol">Novo símbolo.</param>
    public void set_symbol(
        BoardPosition position,
        Symbol symbol)
    {
        cells[position.Row, position.Column] = symbol;
    }

    /// <summary>
    /// Verifica se o símbolo possui uma sequência vencedora.
    /// </summary>
    /// <param name="symbol">Símbolo avaliado.</param>
    /// <returns><see langword="true"/> quando existe vitória.</returns>
    public bool has_winner(Symbol symbol)
    {
        return winning_lines.Any(
            line => line.All(
                position => get_symbol(position) == symbol));
    }

    /// <summary>
    /// Indica se não existem posições livres.
    /// </summary>
    public bool IsFull => get_available_positions().Count == 0;
}
