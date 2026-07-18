namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Define um exportador de registros para CSV.
/// </summary>
/// <typeparam name="T">Tipo de registro exportado.</typeparam>
public interface ICsvExporter<in T>
{
    /// <summary>
    /// Exporta os registros para o caminho informado.
    /// </summary>
    void export(
        string path,
        IEnumerable<T> records);
}
