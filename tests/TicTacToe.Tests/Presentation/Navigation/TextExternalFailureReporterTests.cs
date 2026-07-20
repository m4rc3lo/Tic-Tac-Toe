
using TicTacToe.Persistence;
using TicTacToe.Presentation.Navigation;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

public class TextExternalFailureReporterTests
{
    [Fact]
    public void report_should_not_expose_inner_exception_message()
    {
        StringWriter writer = new();
        InfrastructureOperationException exception = new(
            "gravar",
            "Não foi possível salvar.",
            new IOException(
                @"C:\Users\Pessoa\dado-secreto.json"));

        new TextExternalFailureReporter(writer)
            .report("Falha.", exception);

        string output = writer.ToString();

        Assert.Contains(
            exception.DiagnosticId.ToString("N"),
            output);
        Assert.DoesNotContain("dado-secreto", output);
    }
}
