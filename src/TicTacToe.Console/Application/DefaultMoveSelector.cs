using TicTacToe.AI;
using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Seleciona jogadas humanas pela porta de entrada e jogadas computacionais
/// por uma Strategy resolvida na camada de aplicação.
/// </summary>
public sealed class DefaultMoveSelector : IMoveSelector
{
    private readonly IGameInput game_input;
    private readonly IComputerMoveStrategyResolver strategy_resolver;

    /// <summary>
    /// Inicializa o seletor padrão.
    /// </summary>
    /// <param name="game_input">Porta de entrada para participantes humanos.</param>
    /// <param name="strategy_resolver">
    /// Serviço que associa participantes computacionais às estratégias.
    /// </param>
    public DefaultMoveSelector(
        IGameInput game_input,
        IComputerMoveStrategyResolver strategy_resolver)
    {
        ArgumentNullException.ThrowIfNull(game_input);
        ArgumentNullException.ThrowIfNull(strategy_resolver);

        this.game_input = game_input;
        this.strategy_resolver = strategy_resolver;
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">
    /// Lançada quando o tipo concreto de participante não é reconhecido.
    /// </exception>
    public BoardPosition select_move(Match match, Player player)
    {
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(player);

        return player switch
        {
            HumanPlayer human_player =>
                game_input.read_move(match, human_player),

            ComputerPlayer computer_player =>
                choose_computer_move(match, computer_player),

            _ => throw new NotSupportedException(
                $"O tipo de participante {player.GetType().Name} não é suportado.")
        };
    }

    private BoardPosition choose_computer_move(
        Match match,
        ComputerPlayer player)
    {
        IMoveStrategy strategy =
            strategy_resolver.resolve_strategy(player);

        return strategy.choose_move(match.Board, player.Symbol);
    }
}
