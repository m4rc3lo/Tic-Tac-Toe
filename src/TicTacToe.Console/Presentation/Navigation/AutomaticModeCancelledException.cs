namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Interrompe controladamente uma demonstração automática.
/// </summary>
public sealed class AutomaticModeCancelledException : Exception
{
    public AutomaticModeCancelledException()
        : base("A demonstração automática foi cancelada.")
    {
    }
}
