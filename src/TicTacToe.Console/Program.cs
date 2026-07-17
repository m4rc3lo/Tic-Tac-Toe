using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;

namespace TicTacToe;

/// <summary>
/// Define o ponto de composição da aplicação Console.
/// </summary>
public static class Program
{
    /// <summary>
    /// Inicia a máquina de estados da apresentação.
    /// </summary>
    /// <param name="args">Argumentos recebidos pela linha de comando.</param>
    public static void Main(string[] args)
    {
        TextReader reader = global::System.Console.In;
        TextWriter writer = global::System.Console.Out;

        ConsoleBoardRenderer board_renderer = new(writer);
        ConsoleGameInput game_input = new(reader, writer);
        ConsoleGameOutput game_output = new(
            writer,
            board_renderer);

        IMatchSessionRunner match_session_runner =
            new ConsoleMatchSessionRunner(
                game_input,
                game_output);

        IScreen[] screens =
        [
            new SplashScreen(reader, writer),
            new MainMenuScreen(reader, writer),
            new MatchSetupScreen(reader, writer),
            new PlayingScreen(match_session_runner),
            new MatchResultScreen(reader, writer),
            new StatisticsScreen(reader, writer),
            new ExperimentSetupScreen(reader, writer),
            new SettingsScreen(reader, writer),
            new HelpScreen(reader, writer),
            new ExitScreen(writer)
        ];

        ScreenManager screen_manager = new(screens);
        ScreenContext context = new();

        screen_manager.run(
            ScreenState.Splash,
            context);
    }
}
