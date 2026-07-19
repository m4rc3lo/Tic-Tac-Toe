using TicTacToe.AI;
using TicTacToe.Application;
using TicTacToe.Domain;
using TicTacToe.Persistence;
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

public class AutomaticMatchRunnerTests
{
    [Fact]
    public void run_should_complete_without_real_delay_and_persist_when_requested()
    {
        RecordingPersistenceService persistence = new();
        AutomaticMatchRunner runner = create_runner(
            new ContinueControl(),
            persistence);

        AutomaticMatchResult result = runner.run(
            new AutomaticMatchConfiguration(
                StrategyKind.Minimax,
                StrategyKind.Minimax,
                7,
                PersistMatch: true));

        Assert.False(result.WasCancelled);
        Assert.Equal(GameState.Finished, result.Match.State);
        Assert.Equal(7, result.EffectiveSeed);
        Assert.Equal(1, persistence.CallCount);
    }

    [Fact]
    public void run_should_not_persist_when_option_is_disabled()
    {
        RecordingPersistenceService persistence = new();
        AutomaticMatchRunner runner = create_runner(
            new ContinueControl(),
            persistence);

        runner.run(
            new AutomaticMatchConfiguration(
                StrategyKind.Minimax,
                StrategyKind.Minimax,
                null,
                PersistMatch: false));

        Assert.Equal(0, persistence.CallCount);
    }

    [Fact]
    public void run_should_cancel_safely()
    {
        AutomaticMatchRunner runner = create_runner(
            new CancelControl(),
            persistence: null);

        AutomaticMatchResult result = runner.run(
            new AutomaticMatchConfiguration(
                StrategyKind.Random,
                StrategyKind.Random,
                1,
                PersistMatch: false));

        Assert.True(result.WasCancelled);
        Assert.Equal(GameState.InProgress, result.Match.State);
        Assert.Empty(result.Match.Moves);
    }

    private static AutomaticMatchRunner create_runner(
        IAutomaticModeControl control,
        IMatchPersistenceService? persistence)
    {
        PresentationPreferences preferences = new(
            visual_effects: false,
            animation_delay_milliseconds: 0);

        return new AutomaticMatchRunner(
            new StringWriter(),
            new RecordingGameOutput(),
            new ImmediateDelayService(),
            preferences,
            new MoveStrategyFactory(),
            control,
            persistence);
    }

    private sealed class ContinueControl : IAutomaticModeControl
    {
        public AutomaticControlDecision wait_for_turn() =>
            AutomaticControlDecision.Continue;
    }

    private sealed class CancelControl : IAutomaticModeControl
    {
        public AutomaticControlDecision wait_for_turn() =>
            AutomaticControlDecision.Cancel;
    }

    private sealed class RecordingGameOutput : IGameOutput
    {
        public void show_match(Match match) { }
        public void show_invalid_move(
            Player player,
            BoardPosition position,
            string message) { }
        public void show_result(Match match) { }
    }

    private sealed class RecordingPersistenceService
        : IMatchPersistenceService
    {
        public int CallCount { get; private set; }

        public MatchRecord persist(
            MatchPersistenceContext context)
        {
            CallCount++;
            return new MatchRecordMapper().map(context);
        }
    }
}
