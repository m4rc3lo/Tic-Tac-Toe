using TicTacToe.AI;
using Xunit;

namespace TicTacToe.Tests.AI;

/// <summary>
/// Verifica o contrato do gerador pseudoaleatório padrão.
/// </summary>
public class SystemRandomSourceTests
{
    /// <summary>
    /// Confirma que valores produzidos respeitam o intervalo.
    /// </summary>
    [Fact]
    public void next_should_return_value_inside_range()
    {
        SystemRandomSource random_source = new(42);

        for (int sample = 0; sample < 100; sample++)
        {
            int value = random_source.next(7);

            Assert.InRange(value, 0, 6);
        }
    }

    /// <summary>
    /// Confirma que a mesma semente reproduz a sequência.
    /// </summary>
    [Fact]
    public void same_seed_should_reproduce_values()
    {
        SystemRandomSource first_source = new(1234);
        SystemRandomSource second_source = new(1234);

        int[] first_values = Enumerable
            .Range(0, 20)
            .Select(_ => first_source.next(100))
            .ToArray();

        int[] second_values = Enumerable
            .Range(0, 20)
            .Select(_ => second_source.next(100))
            .ToArray();

        Assert.Equal(first_values, second_values);
    }

    /// <summary>
    /// Confirma a rejeição de limites não positivos.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void next_should_reject_non_positive_limit(int max_exclusive)
    {
        SystemRandomSource random_source = new(1);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => random_source.next(max_exclusive));
    }
}
