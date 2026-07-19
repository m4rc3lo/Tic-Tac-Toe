namespace TicTacToe.Application;

/// <summary>
/// Incrementa a semente base pelo índice da execução.
/// </summary>
public sealed class SequentialSeedSequence : ISeedSequence
{
    public int? get_seed(int? base_seed, int run_number)
    {
        if (run_number < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(run_number));
        }

        return base_seed.HasValue
            ? checked(base_seed.Value + run_number - 1)
            : null;
    }
}
