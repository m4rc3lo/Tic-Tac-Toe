namespace TicTacToe.Application;

/// <summary>
/// Produz sementes determinísticas para execuções sucessivas.
/// </summary>
public interface ISeedSequence
{
    int? get_seed(int? base_seed, int run_number);
}
