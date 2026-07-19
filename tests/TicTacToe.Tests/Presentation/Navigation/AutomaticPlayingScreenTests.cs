using TicTacToe.Domain;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

public class AutomaticPlayingScreenTests
{
    [Fact]
    public void show_should_return_to_menu_after_execution()
    {
        Match match = new(
            new ComputerPlayer("X", Symbol.X),
            new ComputerPlayer("O", Symbol.O));
        RecordingRunner runner = new(match);
        AutomaticPlayingScreen screen = new(runner);
        ScreenContext context = new();
        context.set_automatic_match_configuration(
            new AutomaticMatchConfiguration(
                StrategyKind.Minimax,
                StrategyKind.Minimax,
                null,
                PersistMatch: false));

        ScreenTransition transition = screen.show(context);

        Assert.Equal(ScreenState.MainMenu, transition.Target);
        Assert.NotNull(context.LastAutomaticMatchResult);
        Assert.Equal(1, runner.CallCount);
    }

    private sealed class RecordingRunner : IAutomaticMatchRunner
    {
        private readonly Match match;
        public RecordingRunner(Match match) => this.match = match;
        public int CallCount { get; private set; }
        public AutomaticMatchResult run(
            AutomaticMatchConfiguration configuration)
        {
            CallCount++;
            return new AutomaticMatchResult(match, false, configuration.RandomSeed);
        }
    }
}
