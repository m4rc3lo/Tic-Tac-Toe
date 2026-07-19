using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Executa a demonstração automática e retorna ao menu.
/// </summary>
public sealed class AutomaticPlayingScreen : IScreen
{
    private readonly IAutomaticMatchRunner runner;

    public AutomaticPlayingScreen(
        IAutomaticMatchRunner runner)
    {
        ArgumentNullException.ThrowIfNull(runner);
        this.runner = runner;
    }

    public ScreenState State => ScreenState.AutomaticPlaying;

    public ScreenTransition show(ScreenContext context)
    {
        AutomaticMatchConfiguration? configuration =
            context.AutomaticMatchConfiguration;

        if (configuration is null)
        {
            return new ScreenTransition(
                ScreenState.AutomaticSetup);
        }

        AutomaticMatchResult result = runner.run(configuration);
        context.set_last_automatic_match_result(result);

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
