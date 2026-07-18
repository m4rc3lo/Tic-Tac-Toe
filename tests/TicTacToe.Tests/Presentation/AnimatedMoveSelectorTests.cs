using TicTacToe.Application;
using TicTacToe.Domain;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public class AnimatedMoveSelectorTests
{
    [Fact]
    public void select_move_should_show_indicator_for_computer()
    {
        RecordingAnimationService animation = new();
        AnimatedMoveSelector selector = new(
            new FixedMoveSelector(new BoardPosition(1, 1)),
            animation);
        Match match = new(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));

        BoardPosition position =
            selector.select_move(
                match,
                match.SecondPlayer);

        Assert.Equal(new BoardPosition(1, 1), position);
        Assert.Equal(1, animation.AnalysisCalls);
    }

    [Fact]
    public void select_move_should_not_show_indicator_for_human()
    {
        RecordingAnimationService animation = new();
        AnimatedMoveSelector selector = new(
            new FixedMoveSelector(new BoardPosition(0, 0)),
            animation);
        Match match = new(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));

        selector.select_move(
            match,
            match.FirstPlayer);

        Assert.Equal(0, animation.AnalysisCalls);
    }

    private sealed class FixedMoveSelector : IMoveSelector
    {
        private readonly BoardPosition position;

        public FixedMoveSelector(BoardPosition position)
        {
            this.position = position;
        }

        public BoardPosition select_move(
            Match match,
            Player player)
        {
            return position;
        }
    }

    private sealed class RecordingAnimationService
        : IAnimationService
    {
        public int AnalysisCalls { get; private set; }

        public void write_progressive(
            string text,
            TimeSpan character_delay,
            bool append_line = true)
        {
        }

        public void show_analysis_indicator(
            string message,
            int frame_count,
            TimeSpan frame_delay)
        {
            AnalysisCalls++;
        }

        public void show_progress_bar(
            int current,
            int total,
            int width = 20)
        {
        }
    }
}
