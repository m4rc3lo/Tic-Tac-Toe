using TicTacToe.Application;
using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Apresenta estados e resultados de uma partida em um fluxo textual.
/// </summary>
public sealed class ConsoleGameOutput : IGameOutput
{
    private readonly TextWriter writer;
    private readonly ConsoleBoardRenderer board_renderer;

    /// <summary>
    /// Inicializa a saída textual.
    /// </summary>
    /// <param name="writer">Destino das mensagens.</param>
    /// <param name="board_renderer">Renderizador de tabuleiro.</param>
    public ConsoleGameOutput(
        TextWriter writer,
        ConsoleBoardRenderer board_renderer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(board_renderer);

        this.writer = writer;
        this.board_renderer = board_renderer;
    }

    /// <inheritdoc />
    public void show_match(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);

        writer.WriteLine();
        board_renderer.render(match.Board);

        if (match.State == GameState.InProgress)
        {
            writer.WriteLine();
            writer.WriteLine(
                $"Jogador atual: {match.CurrentPlayer.Name} " +
                $"({match.CurrentPlayer.Symbol})");
        }
    }

    /// <inheritdoc />
    public void show_invalid_move(
        Player player,
        BoardPosition position,
        string message)
    {
        ArgumentNullException.ThrowIfNull(player);

        writer.WriteLine(
            $"Jogada inválida de {player.Name} " +
            $"na posição ({position.Row + 1}, {position.Column + 1}): " +
            message);
    }

    /// <inheritdoc />
    public void show_result(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);

        writer.WriteLine();

        switch (match.Result)
        {
            case GameResult.XWins:
                show_winner(match, Symbol.X);
                break;

            case GameResult.OWins:
                show_winner(match, Symbol.O);
                break;

            case GameResult.Draw:
                writer.WriteLine("Resultado: empate.");
                break;

            default:
                writer.WriteLine("Resultado ainda não definido.");
                break;
        }
    }

    private void show_winner(
        Match match,
        Symbol symbol)
    {
        Player winner = match.get_player(symbol);

        writer.WriteLine(
            $"Resultado: {winner.Name} venceu com {symbol}.");
    }
}
