namespace TicTacToe.Presentation;

/// <summary>
/// Fornece artes textuais da aplicação com variantes Unicode e ASCII.
/// </summary>
public sealed class AsciiArtCatalog
{
    private static readonly string[] unicode_logo =
    [
        "╔══════════════════════════════╗",
        "║   TIC-TAC-TOE  CONSOLE AI   ║",
        "╚══════════════════════════════╝"
    ];

    private static readonly string[] ascii_logo =
    [
        "+------------------------------+",
        "|   TIC-TAC-TOE  CONSOLE AI   |",
        "+------------------------------+"
    ];

    private static readonly string[] victory =
    [
        @" __     _____ _____ ___  ____  ___    _    ",
        @" \ \   / /_ _|_   _/ _ \|  _ \|_ _|  / \   ",
        @"  \ \ / / | |  | || | | | |_) || |  / _ \  ",
        @"   \ V /  | |  | || |_| |  _ < | | / ___ \ ",
        @"    \_/  |___| |_| \___/|_| \_\___/_/   \_\"
    ];

    private static readonly string[] defeat =
    [
        @"  ____  _____ ____  ____   ___ _____  _    ",
        @" |  _ \| ____|  _ \|  _ \ / _ \_   _|/ \   ",
        @" | | | |  _| | |_) | |_) | | | || | / _ \  ",
        @" | |_| | |___|  _ <|  _ <| |_| || |/ ___ \ ",
        @" |____/|_____|_| \_\_| \_\\___/ |_/_/   \_\"
    ];

    private static readonly string[] draw =
    [
        @"  _____ __  __ ____   _  _____ _____ ",
        @" | ____|  \/  |  _ \ / \|_   _| ____|",
        @" |  _| | |\/| | |_) / _ \ | | |  _|  ",
        @" | |___| |  | |  __/ ___ \| | | |___ ",
        @" |_____|_|  |_|_| /_/   \_\_| |_____|"
    ];

    public IReadOnlyList<string> get_logo(bool use_unicode)
    {
        return use_unicode ? unicode_logo : ascii_logo;
    }

    public IReadOnlyList<string> get_victory()
    {
        return victory;
    }

    public IReadOnlyList<string> get_defeat()
    {
        return defeat;
    }

    public IReadOnlyList<string> get_draw()
    {
        return draw;
    }
}
