namespace TicTacToe;

/// <summary>
/// Define o ponto de entrada da aplicação Console.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        new ConsoleApplication(
            global::System.Console.In,
            global::System.Console.Out).run();
    }
}
