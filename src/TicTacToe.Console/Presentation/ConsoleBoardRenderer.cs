using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Renderiza um tabuleiro somente para leitura em formato ASCII.
/// </summary>
public sealed class ConsoleBoardRenderer
{
    private readonly TextWriter writer;

    /// <summary>
    /// Inicializa o renderizador.
    /// </summary>
    /// <param name="writer">Destino textual da renderização.</param>
    public ConsoleBoardRenderer(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    /// <summary>
    /// Escreve coordenadas, casas e separadores do tabuleiro.
    /// </summary>
    /// <param name="board">Tabuleiro consultado.</param>
    public void render(IReadOnlyBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);

        writer.WriteLine("    1   2   3");

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            writer.Write($"{row + 1}   ");

            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                BoardPosition position = new(row, column);
                writer.Write(format_symbol(board.get_symbol(position)));

                if (column < BoardPosition.BoardSize - 1)
                {
                    writer.Write(" | ");
                }
            }

            writer.WriteLine();

            if (row < BoardPosition.BoardSize - 1)
            {
                writer.WriteLine("   ---+---+---");
            }
        }
    }

    private static char format_symbol(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.X => 'X',
            Symbol.O => 'O',
            _ => ' '
        };
    }
}
