namespace TicTacToe.Application;

/// <summary>
/// Persiste o estado acumulado de um experimento.
/// </summary>
public interface IExperimentResultRepository
{
    void save(string output_directory, ExperimentResult result);
}
