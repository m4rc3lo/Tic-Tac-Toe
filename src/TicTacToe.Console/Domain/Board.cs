namespace TicTacToe.Domain;

/// <summary>
/// Representa o tabuleiro 3 × 3 do jogo da velha.
/// </summary>
/// <remarks>
/// A classe encapsula completamente o armazenamento das casas. Operações de
/// apresentação, inteligência artificial e persistência não fazem parte de sua
/// responsabilidade.
/// </remarks>
public sealed class Board
{
    private readonly Symbol[,] cells;
    private int occupied_count;

    /// <summary>
    /// Inicializa um tabuleiro vazio.
    /// </summary>
    public Board()
    {
        cells = new Symbol[BoardPosition.BoardSize, BoardPosition.BoardSize];
        occupied_count = 0;
    }

    /// <summary>
    /// Obtém a quantidade de casas ocupadas.
    /// </summary>
    public int OccupiedCount => occupied_count;

    /// <summary>
    /// Obtém um valor que indica se todas as casas estão ocupadas.
    /// </summary>
    public bool IsFull => occupied_count == BoardPosition.BoardSize * BoardPosition.BoardSize;

    /// <summary>
    /// Obtém o símbolo armazenado em uma posição.
    /// </summary>
    /// <param name="position">Posição consultada.</param>
    /// <returns>
    /// O símbolo da posição, ou <see cref="Symbol.Empty"/> quando a casa está livre.
    /// </returns>
    public Symbol get_symbol(BoardPosition position)
    {
        return cells[position.Row, position.Column];
    }

    /// <summary>
    /// Verifica se uma posição está disponível para receber uma jogada.
    /// </summary>
    /// <param name="position">Posição consultada.</param>
    /// <returns>
    /// <see langword="true"/> quando a posição está vazia; caso contrário,
    /// <see langword="false"/>.
    /// </returns>
    public bool is_position_available(BoardPosition position)
    {
        return get_symbol(position) == Symbol.Empty;
    }

    /// <summary>
    /// Retorna uma cópia das posições atualmente disponíveis.
    /// </summary>
    /// <returns>
    /// Coleção independente do armazenamento interno, ordenada por linha e coluna.
    /// </returns>
    public IReadOnlyList<BoardPosition> get_available_positions()
    {
        List<BoardPosition> available_positions = [];

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            for (int column = 0; column < BoardPosition.BoardSize; column++)
            {
                BoardPosition position = new(row, column);

                if (is_position_available(position))
                {
                    available_positions.Add(position);
                }
            }
        }

        return available_positions.AsReadOnly();
    }

    /// <summary>
    /// Aplica uma jogada em uma casa disponível.
    /// </summary>
    /// <param name="move">Jogada a ser aplicada.</param>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando a posição da jogada já está ocupada.
    /// </exception>
    public void apply_move(Move move)
    {
        if (!is_position_available(move.Position))
        {
            throw new InvalidOperationException(
                $"A posição ({move.Position.Row}, {move.Position.Column}) já está ocupada.");
        }

        cells[move.Position.Row, move.Position.Column] = move.Symbol;
        occupied_count++;
    }

    /// <summary>
    /// Remove a jogada existente em uma posição.
    /// </summary>
    /// <param name="position">Posição cuja jogada será removida.</param>
    /// <returns>O símbolo removido da posição.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando a posição informada já está vazia.
    /// </exception>
    public Symbol undo_move(BoardPosition position)
    {
        Symbol removed_symbol = get_symbol(position);

        if (removed_symbol == Symbol.Empty)
        {
            throw new InvalidOperationException(
                $"A posição ({position.Row}, {position.Column}) já está vazia.");
        }

        cells[position.Row, position.Column] = Symbol.Empty;
        occupied_count--;

        return removed_symbol;
    }
}
