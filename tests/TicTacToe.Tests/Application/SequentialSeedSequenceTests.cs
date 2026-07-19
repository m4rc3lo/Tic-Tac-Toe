using TicTacToe.Application;
using Xunit;

namespace TicTacToe.Tests.Application;

public class SequentialSeedSequenceTests
{
    [Fact]
    public void get_seed_should_increment_base_seed()
    {
        SequentialSeedSequence sequence = new();

        Assert.Equal(40, sequence.get_seed(40, 1));
        Assert.Equal(41, sequence.get_seed(40, 2));
        Assert.Equal(42, sequence.get_seed(40, 3));
        Assert.Null(sequence.get_seed(null, 1));
    }
}
