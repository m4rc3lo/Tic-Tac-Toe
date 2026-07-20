
using TicTacToe.Composition;

namespace TicTacToe;

/// <summary>
/// Executa a aplicação Console a partir do composition root.
/// </summary>
public sealed class ConsoleApplication
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public ConsoleApplication(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public void run()
    {
        ConsoleApplicationRuntime runtime =
            new ConsoleApplicationComposer(
                reader,
                writer)
            .compose();

        runtime.run();
    }
}
