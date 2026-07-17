using TicTacToe.Presentation.Navigation;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

/// <summary>
/// Verifica execução, transições e limites do gerenciador de telas.
/// </summary>
public class ScreenManagerTests
{
    /// <summary>
    /// Confirma uma sequência válida até a saída.
    /// </summary>
    [Fact]
    public void run_should_follow_valid_transitions()
    {
        List<ScreenState> execution_order = [];

        ScreenManager manager = new(
            [
                new StubScreen(
                    ScreenState.Splash,
                    ScreenState.MainMenu,
                    execution_order),
                new StubScreen(
                    ScreenState.MainMenu,
                    ScreenState.Exit,
                    execution_order),
                new StubScreen(
                    ScreenState.Exit,
                    ScreenState.Exit,
                    execution_order)
            ]);

        ScreenState final_state = manager.run(
            ScreenState.Splash,
            new ScreenContext());

        Assert.Equal(ScreenState.Exit, final_state);
        Assert.Equal(
            [
                ScreenState.Splash,
                ScreenState.MainMenu,
                ScreenState.Exit
            ],
            execution_order);
    }

    /// <summary>
    /// Confirma o retorno de uma tela funcional ao menu.
    /// </summary>
    [Fact]
    public void run_should_allow_return_to_main_menu()
    {
        List<ScreenState> execution_order = [];

        ScreenManager manager = new(
            [
                new StubScreen(
                    ScreenState.MatchResult,
                    ScreenState.MainMenu,
                    execution_order),
                new StubScreen(
                    ScreenState.MainMenu,
                    ScreenState.Exit,
                    execution_order),
                new StubScreen(
                    ScreenState.Exit,
                    ScreenState.Exit,
                    execution_order)
            ]);

        manager.run(
            ScreenState.MatchResult,
            new ScreenContext());

        Assert.Equal(
            [
                ScreenState.MatchResult,
                ScreenState.MainMenu,
                ScreenState.Exit
            ],
            execution_order);
    }

    /// <summary>
    /// Confirma que a tela Exit encerra imediatamente.
    /// </summary>
    [Fact]
    public void run_should_stop_after_exit_screen()
    {
        List<ScreenState> execution_order = [];

        ScreenManager manager = new(
            [
                new StubScreen(
                    ScreenState.Exit,
                    ScreenState.MainMenu,
                    execution_order)
            ]);

        ScreenState final_state = manager.run(
            ScreenState.Exit,
            new ScreenContext());

        Assert.Equal(ScreenState.Exit, final_state);
        Assert.Single(execution_order);
    }

    /// <summary>
    /// Confirma a interrupção de ciclos simulados.
    /// </summary>
    [Fact]
    public void run_should_reject_infinite_transition_loop()
    {
        ScreenManager manager = new(
            [
                new StubScreen(
                    ScreenState.MainMenu,
                    ScreenState.MainMenu,
                    [])
            ],
            max_transitions: 3);

        Assert.Throws<InvalidOperationException>(
            () => manager.run(
                ScreenState.MainMenu,
                new ScreenContext()));
    }

    private sealed class StubScreen : IScreen
    {
        private readonly ScreenState target;
        private readonly List<ScreenState> execution_order;

        public StubScreen(
            ScreenState state,
            ScreenState target,
            List<ScreenState> execution_order)
        {
            State = state;
            this.target = target;
            this.execution_order = execution_order;
        }

        public ScreenState State { get; }

        public ScreenTransition show(ScreenContext context)
        {
            execution_order.Add(State);
            return new ScreenTransition(target);
        }
    }
}
