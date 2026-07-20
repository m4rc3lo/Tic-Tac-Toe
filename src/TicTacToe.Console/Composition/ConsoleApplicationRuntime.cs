
using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Composition;

/// <summary>
/// Contém os componentes necessários para executar a navegação.
/// </summary>
public sealed class ConsoleApplicationRuntime
{
    private readonly ScreenManager screen_manager;
    private readonly ScreenContext screen_context;

    public ConsoleApplicationRuntime(
        ScreenManager screen_manager,
        ScreenContext screen_context)
    {
        ArgumentNullException.ThrowIfNull(screen_manager);
        ArgumentNullException.ThrowIfNull(screen_context);

        this.screen_manager = screen_manager;
        this.screen_context = screen_context;
    }

    public void run()
    {
        screen_manager.run(
            ScreenState.Splash,
            screen_context);
    }
}
