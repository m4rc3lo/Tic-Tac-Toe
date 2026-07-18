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
        render(
            board,
            last_move: null,
            Array.Empty<BoardPosition>());
    }

    public void render(
        IReadOnlyBoard board,
        BoardPosition? last_move,
        IReadOnlyList<BoardPosition> winning_positions)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(winning_positions);

        writer.WriteLine("    1   2   3");

        for (int row = 0; row < BoardPosition.BoardSize; row++)
        {
            writer.Write($"{row + 1}   ");

            for (int column = 0;
                 column < BoardPosition.BoardSize;
                 column++)
            {
                BoardPosition position = new(row, column);
                string symbol = format_symbol(
                    board.get_symbol(position));

                writer.Write(
                    apply_highlight(
                        symbol,
                        position,
                        last_move,
                        winning_positions));

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

    private string apply_highlight(
        string symbol,
        BoardPosition position,
        BoardPosition? last_move,
        IReadOnlyList<BoardPosition> winning_positions)
    {
        if (!theme.Preferences.VisualEffects)
        {
            return symbol;
        }

        if (winning_positions.Contains(position))
        {
            return theme.Preferences.UseAnsiColors
                ? theme.colorize_success(symbol)
                : symbol == " " ? " " : $"*{symbol}*";
        }

        if (last_move.HasValue &&
            last_move.Value == position)
        {
            return theme.Preferences.UseAnsiColors
                ? theme.colorize_accent(symbol)
                : symbol == " " ? " " : $"[{symbol}]";
        }

        return symbol;
    }

    private static string format_symbol(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.X => "X",
            Symbol.O => "O",
            _ => " "
        };
    }
}
