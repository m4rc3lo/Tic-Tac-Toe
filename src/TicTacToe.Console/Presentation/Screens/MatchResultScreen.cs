using TicTacToe.Domain;
using TicTacToe.Presentation.Navigation;

namespace TicTacToe.Presentation.Screens;

/// <summary>
/// Apresenta um resumo da última partida e retorna ao menu.
/// </summary>
public sealed class MatchResultScreen : IScreen
{
    private readonly TextReader reader;
    private readonly TextWriter writer;

    public MatchResultScreen(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    public ScreenState State => ScreenState.MatchResult;

    public ScreenTransition show(ScreenContext context)
    {
        Match? match = context.LastMatch;

        if (match is null)
        {
            return new ScreenTransition(ScreenState.MainMenu);
        }

        writer.WriteLine();
        writer.WriteLine($"Partida encerrada: {match.Result}");
        writer.WriteLine(
            $"Quantidade de jogadas: {match.Moves.Count}");
        writer.WriteLine("Pressione Enter para voltar ao menu.");
        reader.ReadLine();

        return new ScreenTransition(ScreenState.MainMenu);
    }
}
