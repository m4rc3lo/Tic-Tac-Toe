using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Application;

/// <summary>
/// Verifica a seleção de jogadas conforme o tipo de participante.
/// </summary>
public class DefaultMoveSelectorTests
{
    /// <summary>
    /// Confirma que participantes humanos utilizam a porta de entrada.
    /// </summary>
    [Fact]
    public void select_move_should_use_input_for_human_player()
    {
        BoardPosition expected_position = new(1, 2);
        RecordingInput input = new(expected_position);
        RecordingResolver resolver = new(new RecordingStrategy(expected_position));
        DefaultMoveSelector selector = new(input, resolver);
        HumanPlayer human_player = new("Ana", Symbol.X);
        ComputerPlayer computer_player = new("CPU", Symbol.O);
        Match match = new(human_player, computer_player);

        BoardPosition selected_position =
            selector.select_move(match, human_player);

        Assert.Equal(expected_position, selected_position);
        Assert.Same(match, input.ReceivedMatch);
        Assert.Same(human_player, input.ReceivedPlayer);
        Assert.Null(resolver.ReceivedPlayer);
    }

    /// <summary>
    /// Confirma que participantes computacionais usam a Strategy resolvida.
    /// </summary>
    [Fact]
    public void select_move_should_use_resolved_computer_strategy()
    {
        BoardPosition expected_position = new(2, 0);
        RecordingStrategy strategy = new(expected_position);
        RecordingResolver resolver = new(strategy);
        ComputerPlayer computer_player = new("CPU", Symbol.O);
        HumanPlayer human_player = new("Ana", Symbol.X);
        Match match = new(human_player, computer_player);
        DefaultMoveSelector selector = new(
            new RecordingInput(new BoardPosition(0, 0)),
            resolver);

        BoardPosition selected_position =
            selector.select_move(match, computer_player);

        Assert.Equal(expected_position, selected_position);
        Assert.Same(computer_player, resolver.ReceivedPlayer);
        Assert.Same(match.Board, strategy.ReceivedBoard);
        Assert.Equal(Symbol.O, strategy.ReceivedSymbol);
    }

    private sealed class RecordingInput : IGameInput
    {
        private readonly BoardPosition position;

        public RecordingInput(BoardPosition position)
        {
            this.position = position;
        }

        public Match? ReceivedMatch { get; private set; }

        public HumanPlayer? ReceivedPlayer { get; private set; }

        public BoardPosition read_move(
            Match match,
            HumanPlayer player)
        {
            ReceivedMatch = match;
            ReceivedPlayer = player;

            return position;
        }
    }

    private sealed class RecordingResolver
        : IComputerMoveStrategyResolver
    {
        private readonly IMoveStrategy strategy;

        public RecordingResolver(IMoveStrategy strategy)
        {
            this.strategy = strategy;
        }

        public ComputerPlayer? ReceivedPlayer { get; private set; }

        public IMoveStrategy resolve_strategy(ComputerPlayer player)
        {
            ReceivedPlayer = player;
            return strategy;
        }
    }

    private sealed class RecordingStrategy : IMoveStrategy
    {
        private readonly BoardPosition position;

        public RecordingStrategy(BoardPosition position)
        {
            this.position = position;
        }

        public IReadOnlyBoard? ReceivedBoard { get; private set; }

        public Symbol ReceivedSymbol { get; private set; }

        public BoardPosition choose_move(
            IReadOnlyBoard board,
            Symbol symbol)
        {
            ReceivedBoard = board;
            ReceivedSymbol = symbol;

            return position;
        }
    }
}
