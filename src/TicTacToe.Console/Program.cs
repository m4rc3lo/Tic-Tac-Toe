using TicTacToe.ReferenceExperiment;

namespace TicTacToe;

/// <summary>
/// Define o ponto de entrada da aplicação Console.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        if (ReferenceExperimentCommand.try_run(
                args,
                global::System.Console.Out))
        {
            return;
        }

        new ConsoleApplication(
            global::System.Console.In,
            global::System.Console.Out).run();
    }
}
