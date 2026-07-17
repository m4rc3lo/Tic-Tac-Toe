using TicTacToe.Domain;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

/// <summary>
/// Verifica mensagens da saída textual.
/// </summary>
public class ConsoleGameOutputTests
{
    /// <summary>
    /// Confirma a apresentação do jogador atual.
    /// </summary>
    [Fact]
    public void show_match_should_present_board_and_current_player()
    {
        Match match = create_match();
        StringWriter writer = new();
        ConsoleGameOutput output = new(
            writer,
            new ConsoleBoardRenderer(writer));

        output.show_match(match);

        string text = writer.ToString();

        Assert.Contains("1   2   3", text);
        Assert.Contains("Jogador atual: Pessoa (X)", text);
    }

    /// <summary>
    /// Confirma a apresentação do vencedor.
    /// </summary>
    [Fact]
    public void show_result_should_present_winner()
    {
        Match match = create_finished_match();
        StringWriter writer = new();
        ConsoleGameOutput output = new(
            writer,
            new ConsoleBoardRenderer(writer));

        output.show_result(match);

        Assert.Contains(
            "Resultado: Pessoa venceu com X.",
            writer.ToString());
    }

    /// <summary>
    /// Confirma a mensagem de jogada inválida.
    /// </summary>
    [Fact]
    public void show_invalid_move_should_present_position_and_message()
    {
        Match match = create_match();
        StringWriter writer = new();
        ConsoleGameOutput output = new(
            writer,
            new ConsoleBoardRenderer(writer));

        output.show_invalid_move(
            match.FirstPlayer,
            new BoardPosition(1, 2),
            "Posição ocupada.");

        string text = writer.ToString();

        Assert.Contains("Pessoa", text);
        Assert.Contains("(2, 3)", text);
        Assert.Contains("Posição ocupada.", text);
    }

    private static Match create_match()
    {
        return new Match(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));
    }

    private static Match create_finished_match()
    {
        Match match = create_match();

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));
        match.apply_move(new BoardPosition(1, 1));
        match.apply_move(new BoardPosition(0, 2));

        return match;
    }
}
