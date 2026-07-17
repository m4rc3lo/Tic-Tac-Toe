using TicTacToe.Domain;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

/// <summary>
/// Verifica a delegação da tela de partida.
/// </summary>
public class PlayingScreenTests
{
    /// <summary>
    /// Confirma a execução e o armazenamento da partida resultante.
    /// </summary>
    [Fact]
    public void show_should_delegate_match_session()
    {
        Match match = new(
            new HumanPlayer("Ana", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));
        RecordingRunner runner = new(match);
        PlayingScreen screen = new(runner);
        ScreenContext context = new();
        MatchConfiguration configuration = new(
            "Ana",
            StrategyKind.Minimax);
        context.set_match_configuration(configuration);

        ScreenTransition transition =
            screen.show(context);

        Assert.Equal(
            ScreenState.MatchResult,
            transition.Target);
        Assert.Equal(
            configuration,
            runner.ReceivedConfiguration);
        Assert.Same(match, context.LastMatch);
    }

    private sealed class RecordingRunner : IMatchSessionRunner
    {
        private readonly Match match;

        public RecordingRunner(Match match)
        {
            this.match = match;
        }

        public MatchConfiguration? ReceivedConfiguration { get; private set; }

        public Match play(MatchConfiguration configuration)
        {
            ReceivedConfiguration = configuration;
            return match;
        }
    }
}
