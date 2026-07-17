using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Application;

/// <summary>
/// Verifica a associação entre participantes computacionais e estratégias.
/// </summary>
public class ComputerMoveStrategyResolverTests
{
    /// <summary>
    /// Confirma a resolução por símbolo.
    /// </summary>
    [Fact]
    public void resolve_strategy_should_return_configured_strategy()
    {
        IMoveStrategy strategy = new RandomMoveStrategy(2026);
        ConfiguredComputerMoveStrategyResolver resolver = new(
            new Dictionary<Symbol, IMoveStrategy>
            {
                [Symbol.O] = strategy
            });
        ComputerPlayer player = new("CPU", Symbol.O);

        IMoveStrategy resolved_strategy =
            resolver.resolve_strategy(player);

        Assert.Same(strategy, resolved_strategy);
    }

    /// <summary>
    /// Confirma a rejeição de participante sem configuração.
    /// </summary>
    [Fact]
    public void resolve_strategy_should_reject_unconfigured_player()
    {
        ConfiguredComputerMoveStrategyResolver resolver = new(
            new Dictionary<Symbol, IMoveStrategy>());
        ComputerPlayer player = new("CPU", Symbol.X);

        Assert.Throws<InvalidOperationException>(
            () => resolver.resolve_strategy(player));
    }
}
