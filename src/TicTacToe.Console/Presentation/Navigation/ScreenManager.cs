namespace TicTacToe.Presentation.Navigation;

/// <summary>
/// Centraliza a execução e as transições da máquina de estados de apresentação.
/// </summary>
public sealed class ScreenManager
{
    private readonly IReadOnlyDictionary<ScreenState, IScreen> screens;
    private readonly int max_transitions;

    /// <summary>
    /// Inicializa o gerenciador com as telas disponíveis.
    /// </summary>
    /// <param name="screens">Telas indexadas por seus próprios estados.</param>
    /// <param name="max_transitions">
    /// Limite de segurança para detectar ciclos indevidos.
    /// </param>
    public ScreenManager(
        IEnumerable<IScreen> screens,
        int max_transitions = 1000)
    {
        ArgumentNullException.ThrowIfNull(screens);

        if (max_transitions <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(max_transitions),
                max_transitions,
                "O limite de transições deve ser maior que zero.");
        }

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
        this.max_transitions = max_transitions;
    }

    /// <summary>
    /// Executa a máquina de estados até a tela de saída.
    /// </summary>
    /// <param name="initial_state">Estado inicial.</param>
    /// <param name="context">Contexto compartilhado.</param>
    /// <returns>Estado final, sempre <see cref="ScreenState.Exit"/>.</returns>
    public ScreenState run(
        ScreenState initial_state,
        ScreenContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        ScreenState current_state = initial_state;
        int transition_count = 0;

        while (true)
        {
            IScreen screen = get_screen(current_state);
            ScreenTransition transition = screen.show(context);

            if (current_state == ScreenState.Exit)
            {
                return ScreenState.Exit;
            }

            transition_count++;

            if (transition_count > max_transitions)
            {
                throw new InvalidOperationException(
                    "O limite de transições foi excedido. " +
                    "A navegação pode conter um ciclo indevido.");
            }

            get_screen(transition.Target);
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
