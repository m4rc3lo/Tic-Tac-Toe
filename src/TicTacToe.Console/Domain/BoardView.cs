namespace TicTacToe.Domain;

/// <summary>
/// Fornece uma visão somente para leitura de um <see cref="Board"/>.
/// </summary>
internal sealed class BoardView : IReadOnlyBoard
{
    private readonly Board board;

    /// <summary>
    /// Inicializa a visão para o tabuleiro informado.
    /// </summary>
    /// <param name="board">Tabuleiro interno do agregado.</param>
    public BoardView(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);
        this.board = board;
    }

    /// <inheritdoc />
    public int OccupiedCount => board.OccupiedCount;

    /// <inheritdoc />
    public bool IsFull => board.IsFull;

    /// <inheritdoc />
    public Symbol get_symbol(BoardPosition position)
    {
        return board.get_symbol(position);
    }

    /// <inheritdoc />
    public bool is_position_available(BoardPosition position)
    {
        return board.is_position_available(position);
    }

    /// <inheritdoc />
    public IReadOnlyList<BoardPosition> get_available_positions()
    {
        return board.get_available_positions();
    }
}
