using TicTacToe.Presentation;
using TicTacToe.Presentation.Navigation;
using TicTacToe.Presentation.Screens;
using Xunit;

namespace TicTacToe.Tests.Presentation;

public class CreditsScreenTests
{
    [Fact]
    public void show_should_present_citation_metadata_and_return_to_menu()
    {
        StringWriter writer = new();
        CreditsScreen screen = new(
            new StringReader(Environment.NewLine),
            writer,
            new CitationMetadata(
                "Tic-Tac-Toe Console AI",
                "1.6.0",
                "Marcelo Dornbusch Lopes",
                "Apache-2.0",
                "https://github.com/m4rc3lo/Tic-Tac-Toe"));

        ScreenTransition transition =
            screen.show(new ScreenContext());

        Assert.Equal(ScreenState.MainMenu, transition.Target);
        Assert.Contains("Marcelo Dornbusch Lopes", writer.ToString());
        Assert.Contains("CITATION.cff", writer.ToString());
    }
}
