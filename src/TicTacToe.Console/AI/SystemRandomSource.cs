namespace TicTacToe.AI;

/// <summary>
/// Implementa <see cref="IRandomSource"/> utilizando <see cref="Random"/>.
/// </summary>
public sealed class SystemRandomSource : IRandomSource
{
    private readonly Random random;

    /// <summary>
    /// Inicializa um gerador com semente definida pelo ambiente.
    /// </summary>
    public SystemRandomSource()
        : this(new Random())
    {
    }

    /// <summary>
    /// Inicializa um gerador com semente controlável.
    /// </summary>
    /// <param name="seed">Semente utilizada pelo gerador.</param>
    public SystemRandomSource(int seed)
        : this(new Random(seed))
    {
    }

    private SystemRandomSource(Random random)
    {
        this.random = random;
    }

    /// <inheritdoc />
    public int next(int max_exclusive)
    {
        if (max_exclusive <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(max_exclusive),
                max_exclusive,
                "O limite superior deve ser maior que zero.");
        }

        return random.Next(max_exclusive);
    }
}
