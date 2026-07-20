
namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Implementa pausa e cancelamento por teclado no terminal interativo.
/// </summary>
public sealed class ConsoleAutomaticModeControl
    : IAutomaticModeControl
{
    private readonly TextWriter writer;
    private readonly Func<bool> key_available;
    private readonly Func<ConsoleKeyInfo> read_key;
    private readonly IDelayService delay_service;
    private readonly bool supports_interactive_input;

    public ConsoleAutomaticModeControl(
        TextWriter writer,
        IDelayService delay_service)
        : this(
            writer,
            delay_service,
            () => global::System.Console.KeyAvailable,
            () => global::System.Console.ReadKey(intercept: true),
            supports_interactive_input:
                !global::System.Console.IsInputRedirected)
    {
    }

    public ConsoleAutomaticModeControl(
        TextWriter writer,
        IDelayService delay_service,
        Func<bool> key_available,
        Func<ConsoleKeyInfo> read_key,
        bool supports_interactive_input = true)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(delay_service);
        ArgumentNullException.ThrowIfNull(key_available);
        ArgumentNullException.ThrowIfNull(read_key);

        this.writer = writer;
        this.delay_service = delay_service;
        this.key_available = key_available;
        this.read_key = read_key;
        this.supports_interactive_input =
            supports_interactive_input;
    }

    public AutomaticControlDecision wait_for_turn()
    {
        if (!supports_interactive_input)
        {
            return AutomaticControlDecision.Continue;
        }

        bool has_key;

        try
        {
            has_key = key_available();
        }
        catch (InvalidOperationException)
        {
            return AutomaticControlDecision.Continue;
        }
        catch (IOException)
        {
            return AutomaticControlDecision.Continue;
        }

        if (!has_key)
        {
            return AutomaticControlDecision.Continue;
        }

        ConsoleKeyInfo key = read_key();

        if (key.Key == ConsoleKey.Escape)
        {
            return AutomaticControlDecision.Cancel;
        }

        if (key.Key != ConsoleKey.Spacebar)
        {
            return AutomaticControlDecision.Continue;
        }

        writer.WriteLine();
        writer.WriteLine(
            "Demonstração pausada. Espaço continua; Esc cancela.");

        while (true)
        {
            if (key_available())
            {
                key = read_key();

                if (key.Key == ConsoleKey.Escape)
                {
                    return AutomaticControlDecision.Cancel;
                }

                if (key.Key == ConsoleKey.Spacebar)
                {
                    writer.WriteLine("Demonstração retomada.");
                    return AutomaticControlDecision.Continue;
                }
            }

            delay_service.wait(TimeSpan.FromMilliseconds(25));
        }
    }
}
