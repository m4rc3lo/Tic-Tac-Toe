using TicTacToe.Application;
using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Decora a saída da partida com atraso configurável entre jogadas.
/// </summary>
public sealed class DelayedGameOutput : IGameOutput
{
    private readonly IGameOutput inner_output;
    private readonly IDelayService delay_service;
    private readonly PresentationPreferences preferences;

    public DelayedGameOutput(
        IGameOutput inner_output,
        IDelayService delay_service,
        PresentationPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(inner_output);
        ArgumentNullException.ThrowIfNull(delay_service);
        ArgumentNullException.ThrowIfNull(preferences);

        this.inner_output = inner_output;
        this.delay_service = delay_service;
        this.preferences = preferences;
    }

    public void show_match(Match match)
    {
        inner_output.show_match(match);

        if (preferences.VisualEffects &&
            preferences.AnimationDelayMilliseconds > 0 &&
            match.Moves.Count > 0 &&
            match.State == GameState.InProgress)
        {
            delay_service.wait(
                TimeSpan.FromMilliseconds(
                    preferences.AnimationDelayMilliseconds));
        }
    }

    public void show_invalid_move(
        Player player,
        BoardPosition position,
        string message)
    {
        inner_output.show_invalid_move(
            player,
            position,
            message);
    }

    public void show_result(Match match)
    {
        inner_output.show_result(match);
    }
}
