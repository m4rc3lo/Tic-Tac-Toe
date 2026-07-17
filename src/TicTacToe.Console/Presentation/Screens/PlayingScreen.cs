using TicTacToe.Domain;
using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Coordena a execução da partida por meio de um serviço de sessão.
/// </summary>
public sealed class PlayingScreen : IScreen
{
    private readonly IMatchSessionRunner match_session_runner;

    public PlayingScreen(
        IMatchSessionRunner match_session_runner)
    {
        ArgumentNullException.ThrowIfNull(match_session_runner);
        this.match_session_runner = match_session_runner;
    }

    public ScreenState State => ScreenState.Playing;

    public ScreenTransition show(ScreenContext context)
    {
        MatchConfiguration? configuration =
            context.MatchConfiguration;

        if (configuration is null)
        {
            return new ScreenTransition(ScreenState.MatchSetup);
        }

        Match match =
            match_session_runner.play(configuration);

        context.set_last_match(match);

        return new ScreenTransition(ScreenState.MatchResult);
    }
}
