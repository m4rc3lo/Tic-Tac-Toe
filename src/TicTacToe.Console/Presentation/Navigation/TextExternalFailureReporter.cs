
using TicTacToe.Persistence;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Escreve mensagens públicas e identificadores diagnósticos.
/// </summary>
public sealed class TextExternalFailureReporter
    : IExternalFailureReporter
{
    private readonly TextWriter writer;

    public TextExternalFailureReporter(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        this.writer = writer;
    }

    public void report(
        string context,
        Exception exception)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(context);
        ArgumentNullException.ThrowIfNull(exception);

        if (exception is InfrastructureOperationException failure)
        {
            writer.WriteLine(
                $"{context} {failure.Message} " +
                $"Diagnóstico: {failure.DiagnosticId:N}.");
            return;
        }

        writer.WriteLine(
            $"{context} Ocorreu uma falha externa. " +
            "Consulte os registros técnicos.");
    }
}
