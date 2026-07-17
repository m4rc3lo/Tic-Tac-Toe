using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using TicTacToe.Presentation;

namespace TicTacToe;

/// <summary>
/// Define o ponto de composição da aplicação Console.
/// </summary>
public static class Program
{
    /// <summary>
    /// Inicia uma partida mínima entre pessoa e agente Minimax.
    /// </summary>
    /// <param name="args">Argumentos recebidos pela linha de comando.</param>
    public static void Main(string[] args)
    {
        TextReader reader = global::System.Console.In;
        TextWriter writer = global::System.Console.Out;

        ConsoleBoardRenderer board_renderer = new(writer);
        ConsoleGameInput game_input = new(reader, writer);
        ConsoleGameOutput game_output = new(writer, board_renderer);

        IComputerMoveStrategyResolver strategy_resolver =
            new ConfiguredComputerMoveStrategyResolver(
                new Dictionary<Symbol, IMoveStrategy>
                {
                    [Symbol.O] = new MinimaxMoveStrategy()
                });

        IMoveSelector move_selector = new DefaultMoveSelector(
            game_input,
            strategy_resolver);

        MatchController controller = new(
            move_selector,
            game_output);

        Match match = new(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("Minimax", Symbol.O));

        writer.WriteLine("Tic-Tac-Toe Console AI");
        writer.WriteLine("Pessoa: X | Minimax: O");

        controller.run(match);
    }
}
