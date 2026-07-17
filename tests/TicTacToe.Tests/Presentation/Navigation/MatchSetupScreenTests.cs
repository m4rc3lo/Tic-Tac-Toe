using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation.Navigation;

/// <summary>
/// Verifica a coleta da configuração de partida.
/// </summary>
public class MatchSetupScreenTests
{
    /// <summary>
    /// Confirma nome e Strategy escolhidos.
    /// </summary>
    [Fact]
    public void show_should_store_match_configuration()
    {
        StringReader reader = new(
            "Ana" + Environment.NewLine +
            "2" + Environment.NewLine);
        StringWriter writer = new();
        MatchSetupScreen screen = new(
            reader,
            writer);
        ScreenContext context = new();

        ScreenTransition transition =
            screen.show(context);

        Assert.Equal(
            ScreenState.Playing,
            transition.Target);
        Assert.NotNull(context.MatchConfiguration);
        Assert.Equal(
            "Ana",
            context.MatchConfiguration.PlayerName);
        Assert.Equal(
            StrategyKind.Heuristic,
            context.MatchConfiguration.OpponentStrategy);
    }

    /// <summary>
    /// Confirma o retorno ao menu sem configuração.
    /// </summary>
    [Fact]
    public void show_should_return_to_menu_when_cancelled()
    {
        StringReader reader = new(
            Environment.NewLine +
            "0" + Environment.NewLine);
        MatchSetupScreen screen = new(
            reader,
            new StringWriter());
        ScreenContext context = new();

        ScreenTransition transition =
            screen.show(context);

        Assert.Equal(
            ScreenState.MainMenu,
            transition.Target);
        Assert.Null(context.MatchConfiguration);
    }
}
