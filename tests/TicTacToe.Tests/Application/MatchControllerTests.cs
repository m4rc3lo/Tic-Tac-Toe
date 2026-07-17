using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Application;

/// <summary>
/// Verifica o fluxo de aplicação sem interação humana ou Console.
/// </summary>
public class MatchControllerTests
{
    /// <summary>
    /// Confirma a execução completa até uma vitória.
    /// </summary>
    [Fact]
    public void run_should_coordinate_match_until_win()
    {
        Match match = create_match();
        QueueMoveSelector selector = new(
            [
                new BoardPosition(0, 0),
                new BoardPosition(1, 0),
                new BoardPosition(0, 1),
                new BoardPosition(1, 1),
                new BoardPosition(0, 2)
            ]);
        RecordingOutput output = new();
        MatchController controller = new(selector, output);

        Match result = controller.run(match);

        Assert.Same(match, result);
        Assert.Equal(GameState.Finished, match.State);
        Assert.Equal(GameResult.XWins, match.Result);
        Assert.Equal(5, match.Moves.Count);
        Assert.Equal(6, output.ShownMatches.Count);
        Assert.Single(output.Results);
        Assert.Empty(output.InvalidMoves);
    }

    /// <summary>
    /// Confirma que uma jogada inválida é comunicada e o fluxo continua.
    /// </summary>
    [Fact]
    public void run_should_report_invalid_move_and_continue()
    {
        Match match = create_match();
        QueueMoveSelector selector = new(
            [
                new BoardPosition(0, 0),
                new BoardPosition(0, 0),
                new BoardPosition(1, 0),
                new BoardPosition(0, 1),
                new BoardPosition(1, 1),
                new BoardPosition(0, 2)
            ]);
        RecordingOutput output = new();
        MatchController controller = new(selector, output);

        controller.run(match);

        Assert.Equal(GameResult.XWins, match.Result);
        Assert.Single(output.InvalidMoves);
        Assert.Equal(Symbol.O, output.InvalidMoves[0].Player.Symbol);
        Assert.Equal(
            new BoardPosition(0, 0),
            output.InvalidMoves[0].Position);
        Assert.Equal(5, match.Moves.Count);
    }

    /// <summary>
    /// Confirma a execução automática entre participantes simulados.
    /// </summary>
    [Fact]
    public void run_should_not_require_human_interaction()
    {
        ComputerPlayer first_player = new("CPU X", Symbol.X);
        ComputerPlayer second_player = new("CPU O", Symbol.O);
        Match match = new(first_player, second_player);
        DefaultMoveSelector selector = new(
            new FailingInput(),
            new ConfiguredComputerMoveStrategyResolver(
                new Dictionary<Symbol, IMoveStrategy>
                {
                    [Symbol.X] = new FirstAvailableStrategy(),
                    [Symbol.O] = new FirstAvailableStrategy()
                }));
        RecordingOutput output = new();
        MatchController controller = new(selector, output);

        controller.run(match);

        Assert.Equal(GameState.Finished, match.State);
        Assert.NotEqual(GameResult.None, match.Result);
        Assert.InRange(match.Moves.Count, 5, 9);
        Assert.Single(output.Results);
    }

    /// <summary>
    /// Confirma que falhas da saída não são tratadas como jogadas inválidas.
    /// </summary>
    [Fact]
    public void run_should_propagate_output_failure()
    {
        Match match = create_match();
        QueueMoveSelector selector = new(
            [new BoardPosition(0, 0)]);
        MatchController controller = new(
            selector,
            new FailingOutput());

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () => controller.run(match));

        Assert.Equal("Falha de apresentação.", exception.Message);
        Assert.Single(match.Moves);
    }

    /// <summary>
    /// Confirma que partidas já encerradas são rejeitadas.
    /// </summary>
    [Fact]
    public void run_should_reject_finished_match()
    {
        Match match = create_finished_match();
        MatchController controller = new(
            new QueueMoveSelector([]),
            new RecordingOutput());

        Assert.Throws<InvalidOperationException>(
            () => controller.run(match));
    }

    private static Match create_match()
    {
        return new Match(
            new HumanPlayer("Ana", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));
    }

    private static Match create_finished_match()
    {
        Match match = create_match();

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));
        match.apply_move(new BoardPosition(1, 1));
        match.apply_move(new BoardPosition(0, 2));

        return match;
    }

    private sealed class QueueMoveSelector : IMoveSelector
    {
        private readonly Queue<BoardPosition> positions;

        public QueueMoveSelector(
            IEnumerable<BoardPosition> positions)
        {
            this.positions = new Queue<BoardPosition>(positions);
        }

        public BoardPosition select_move(
            Match match,
            Player player)
        {
            return positions.Dequeue();
        }
    }

    private sealed class RecordingOutput : IGameOutput
    {
        public List<Match> ShownMatches { get; } = [];

        public List<InvalidMoveRecord> InvalidMoves { get; } = [];

        public List<Match> Results { get; } = [];

        public void show_match(Match match)
        {
            ShownMatches.Add(match);
        }

        public void show_invalid_move(
            Player player,
            BoardPosition position,
            string message)
        {
            InvalidMoves.Add(
                new InvalidMoveRecord(player, position, message));
        }

        public void show_result(Match match)
        {
            Results.Add(match);
        }
    }

    private sealed record InvalidMoveRecord(
        Player Player,
        BoardPosition Position,
        string Message);

    private sealed class FirstAvailableStrategy : IMoveStrategy
    {
        public BoardPosition choose_move(
            IReadOnlyBoard board,
            Symbol symbol)
        {
            return board.get_available_positions()[0];
        }
    }

    private sealed class FailingOutput : IGameOutput
    {
        private int show_count;

        public void show_match(Match match)
        {
            show_count++;

            if (show_count > 1)
            {
                throw new InvalidOperationException(
                    "Falha de apresentação.");
            }
        }

        public void show_invalid_move(
            Player player,
            BoardPosition position,
            string message)
        {
            throw new InvalidOperationException(
                "Uma falha de saída não deve virar jogada inválida.");
        }

        public void show_result(Match match)
        {
        }
    }

    private sealed class FailingInput : IGameInput
    {
        public BoardPosition read_move(
            Match match,
            HumanPlayer player)
        {
            throw new InvalidOperationException(
                "A entrada humana não deveria ser utilizada.");
        }
    }
}
