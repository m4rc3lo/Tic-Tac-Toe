
namespace TicTacToe.Persistence;

/// <summary>
/// Representa uma falha operacional de infraestrutura com contexto seguro.
/// </summary>
public sealed class InfrastructureOperationException : Exception
{
    public InfrastructureOperationException(
        string operation,
        string user_message,
        Exception inner_exception)
        : base(user_message, inner_exception)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(operation);
        ArgumentException.ThrowIfNullOrWhiteSpace(user_message);

        Operation = operation;
        DiagnosticId = Guid.NewGuid();
    }

    public string Operation { get; }

    public Guid DiagnosticId { get; }
}
