using TicTacToe.AI;
using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Resolve estratégias configuradas por símbolo.
/// </summary>
public sealed class ConfiguredComputerMoveStrategyResolver
    : IComputerMoveStrategyResolver
{
    private readonly IReadOnlyDictionary<Symbol, IMoveStrategy> strategies;

    /// <summary>
    /// Inicializa o resolvedor com as estratégias disponíveis.
    /// </summary>
    /// <param name="strategies">Mapeamento entre símbolo e Strategy.</param>
    public ConfiguredComputerMoveStrategyResolver(
        IReadOnlyDictionary<Symbol, IMoveStrategy> strategies)
    {
        ArgumentNullException.ThrowIfNull(strategies);
        this.strategies = strategies;
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Lançada quando não há estratégia configurada para o participante.
    /// </exception>
    public IMoveStrategy resolve_strategy(ComputerPlayer player)
    {
        ArgumentNullException.ThrowIfNull(player);

        if (!strategies.TryGetValue(player.Symbol, out IMoveStrategy? strategy))
        {
            throw new InvalidOperationException(
                $"Não existe estratégia configurada para {player.Symbol}.");
        }

        return strategy;
    }
}
