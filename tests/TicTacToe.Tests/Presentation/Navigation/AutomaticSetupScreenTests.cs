using TicTacToe.Persistence;
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

public class AutomaticSetupScreenTests
{
    [Fact]
    public void show_should_use_default_strategy_and_seed()
    {
        ApplicationSettings settings = new()
        {
            DefaultStrategy = "Heuristic",
            RandomSeed = 123
        };
        ScreenContext context = new(
            PresentationPreferences.from_settings(settings),
            settings,
            settings_repository: null);
        AutomaticSetupScreen screen = new(
            new StringReader(
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "n" + Environment.NewLine),
            new StringWriter());

        ScreenTransition transition = screen.show(context);

        Assert.Equal(ScreenState.AutomaticPlaying, transition.Target);
        Assert.NotNull(context.AutomaticMatchConfiguration);
        Assert.Equal(
            StrategyKind.Heuristic,
            context.AutomaticMatchConfiguration.XStrategy);
        Assert.Equal(
            StrategyKind.Heuristic,
            context.AutomaticMatchConfiguration.OStrategy);
        Assert.Equal(123, context.AutomaticMatchConfiguration.RandomSeed);
        Assert.False(context.AutomaticMatchConfiguration.PersistMatch);
    }

    [Fact]
    public void show_should_allow_distinct_strategies_and_persistence()
    {
        ScreenContext context = new();
        AutomaticSetupScreen screen = new(
            new StringReader(
                "1" + Environment.NewLine +
                "3" + Environment.NewLine +
                "42" + Environment.NewLine +
                "s" + Environment.NewLine),
            new StringWriter());

        screen.show(context);

        Assert.Equal(
            StrategyKind.Random,
            context.AutomaticMatchConfiguration!.XStrategy);
        Assert.Equal(
            StrategyKind.Minimax,
            context.AutomaticMatchConfiguration.OStrategy);
        Assert.Equal(42, context.AutomaticMatchConfiguration.RandomSeed);
        Assert.True(context.AutomaticMatchConfiguration.PersistMatch);
    }
}
