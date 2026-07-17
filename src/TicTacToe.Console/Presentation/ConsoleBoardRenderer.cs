using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Renderiza um tabuleiro somente para leitura em Unicode ou ASCII.
/// </summary>
public sealed class ConsoleBoardRenderer
{
    private readonly TextWriter writer;
    private readonly ConsoleTheme theme;

    public ConsoleBoardRenderer(TextWriter writer)
        : this(
            writer,
            new ConsoleTheme(
                new PresentationPreferences(
                    use_unicode: false,
                    visual_effects: false)))
    {
    }

    public ConsoleBoardRenderer(
        TextWriter writer,
        ConsoleTheme theme)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(theme);

        this.writer = writer;
        this.theme = theme;
    }

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
                    writer.Write(
                        $" {theme.VerticalSeparator} ");
                }
            }

            writer.WriteLine();

            if (row < BoardPosition.BoardSize - 1)
            {
                writer.WriteLine(
                    $"   {theme.HorizontalSeparator}");
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
