
namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Centraliza a execução e as transições da máquina de estados.
/// </summary>
public sealed class ScreenManager
{
    private readonly IReadOnlyDictionary<ScreenState, IScreen> screens;
    private readonly INavigationCycleDetector? cycle_detector;

    public ScreenManager(
        IEnumerable<IScreen> screens)
        : this(screens, cycle_detector: null)
    {
    }

    public ScreenManager(
        IEnumerable<IScreen> screens,
        int max_transitions)
        : this(
            screens,
            new TransitionLimitCycleDetector(max_transitions))
    {
    }

    public ScreenManager(
        IEnumerable<IScreen> screens,
        INavigationCycleDetector? cycle_detector)
    {
        ArgumentNullException.ThrowIfNull(screens);

        Dictionary<ScreenState, IScreen> screen_map = [];

        foreach (IScreen screen in screens)
        {
            ArgumentNullException.ThrowIfNull(screen);

            if (!screen_map.TryAdd(screen.State, screen))
            {
                throw new ArgumentException(
                    $"O estado {screen.State} possui mais de uma tela.",
                    nameof(screens));
            }
        }

        this.screens = screen_map;
        this.cycle_detector = cycle_detector;
    }

    public ScreenState run(
        ScreenState initial_state,
        ScreenContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        ScreenState current_state = initial_state;

        while (true)
        {
            IScreen screen = get_screen(current_state);
            ScreenTransition transition = screen.show(context);

            if (current_state == ScreenState.Exit)
            {
                return ScreenState.Exit;
            }

            get_screen(transition.Target);
            cycle_detector?.observe(
                current_state,
                transition.Target);
            current_state = transition.Target;
        }
    }

    private IScreen get_screen(ScreenState state)
    {
        if (!screens.TryGetValue(state, out IScreen? screen))
        {
            throw new InvalidOperationException(
                $"Não existe tela registrada para o estado {state}.");
        }

        return screen;
    }
}
