using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;

namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Compõe e executa uma partida usando os adaptadores textuais existentes.
/// </summary>
public sealed class ConsoleMatchSessionRunner : IMatchSessionRunner
{
    private readonly IGameInput game_input;
    private readonly IGameOutput game_output;

    /// <summary>
    /// Inicializa o executor de sessões.
    /// </summary>
    /// <param name="game_input">Entrada utilizada nos turnos humanos.</param>
    /// <param name="game_output">Saída utilizada durante a partida.</param>
    public ConsoleMatchSessionRunner(
        IGameInput game_input,
        IGameOutput game_output)
    {
        ArgumentNullException.ThrowIfNull(game_input);
        ArgumentNullException.ThrowIfNull(game_output);

        this.game_input = game_input;
        this.game_output = game_output;
    }

    /// <inheritdoc />
    public Match play(MatchConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        IMoveStrategy strategy =
            create_strategy(configuration.OpponentStrategy);

        IComputerMoveStrategyResolver strategy_resolver =
            new ConfiguredComputerMoveStrategyResolver(
                new Dictionary<Symbol, IMoveStrategy>
                {
                    [Symbol.O] = strategy
                });

        IMoveSelector move_selector = new DefaultMoveSelector(
            game_input,
            strategy_resolver);

        MatchController controller = new(
            move_selector,
            game_output);

        Match match = new(
            new HumanPlayer(
                configuration.PlayerName,
                Symbol.X),
            new ComputerPlayer(
                configuration.OpponentStrategy.ToString(),
                Symbol.O));

        return controller.run(match);
    }

    private static IMoveStrategy create_strategy(
        StrategyKind strategy_kind)
    {
        return strategy_kind switch
        {
            StrategyKind.Random => new RandomMoveStrategy(),
            StrategyKind.Heuristic => new HeuristicMoveStrategy(),
            StrategyKind.Minimax => new MinimaxMoveStrategy(),
            _ => throw new ArgumentOutOfRangeException(
                nameof(strategy_kind),
                strategy_kind,
                "A estratégia selecionada não é suportada.")
        };
    }
}
