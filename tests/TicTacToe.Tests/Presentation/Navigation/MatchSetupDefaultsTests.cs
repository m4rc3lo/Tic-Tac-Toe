using TicTacToe.Persistence;
using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

public class MatchSetupDefaultsTests
{
    [Fact]
    public void show_should_apply_configured_strategy_and_seed_on_blank_option()
    {
        ApplicationSettings settings = new()
        {
            DefaultStrategy = "Random",
            RandomSeed = 77
        };
        ScreenContext context = new(
            PresentationPreferences.from_settings(settings),
            settings,
            settings_repository: null);
        MatchSetupScreen screen = new(
            new StringReader(
                Environment.NewLine +
                Environment.NewLine),
            new StringWriter());

        screen.show(context);

        Assert.Equal(
            StrategyKind.Random,
            context.MatchConfiguration!.OpponentStrategy);
        Assert.Equal(77, context.MatchConfiguration.RandomSeed);
    }
}
