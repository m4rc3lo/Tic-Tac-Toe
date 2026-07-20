
namespace TicTacToe.Persistence;

/// <summary>
/// Representa o resultado imutável da validação de configurações.
/// </summary>
public sealed record SettingsValidationResult
{
    public SettingsValidationResult(
        bool is_valid,
        IEnumerable<string> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        IsValid = is_valid;
        Errors = Array.AsReadOnly(errors.ToArray());
    }

    public bool IsValid { get; }
    public IReadOnlyList<string> Errors { get; }
}
