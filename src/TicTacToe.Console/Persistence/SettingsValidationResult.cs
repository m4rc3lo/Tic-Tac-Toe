namespace TicTacToe.Persistence;

/// <summary>
/// Representa o resultado imutável da validação de configurações.
/// </summary>
/// <param name="IsValid">Indica se todos os valores são válidos.</param>
/// <param name="Errors">Erros encontrados.</param>
public sealed record SettingsValidationResult(
    bool IsValid,
    IReadOnlyList<string> Errors);
