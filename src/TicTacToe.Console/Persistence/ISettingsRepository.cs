namespace TicTacToe.Persistence;

/// <summary>
/// Define leitura e gravação das configurações da aplicação.
/// </summary>
public interface ISettingsRepository
{
    /// <summary>
    /// Carrega configurações válidas ou valores padrão seguros.
    /// </summary>
    ApplicationSettings load();

    /// <summary>
    /// Valida e persiste as configurações.
    /// </summary>
    void save(ApplicationSettings settings);
}
