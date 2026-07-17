using TicTacToe.AI;
using TicTacToe.Domain;
using TicTacToe.Presentation;
using Xunit;

namespace TicTacToe.Tests.Presentation;

/// <summary>
/// Verifica parsing e repetição da entrada textual.
/// </summary>
public class ConsoleGameInputTests
{
    /// <summary>
    /// Confirma coordenadas separadas por espaço.
    /// </summary>
    [Fact]
    public void try_parse_position_should_accept_valid_coordinates()
    {
        bool parsed = ConsoleGameInput.try_parse_position(
            "2 3",
            out BoardPosition position);

        Assert.True(parsed);
        Assert.Equal(new BoardPosition(1, 2), position);
    }

    /// <summary>
    /// Confirma separadores alternativos suportados.
    /// </summary>
    [Theory]
    [InlineData("1,3", 0, 2)]
    [InlineData("3;1", 2, 0)]
    [InlineData(" 2\t2 ", 1, 1)]
    public void try_parse_position_should_accept_supported_separators(
        string input,
        int expected_row,
        int expected_column)
    {
        bool parsed = ConsoleGameInput.try_parse_position(
            input,
            out BoardPosition position);

        Assert.True(parsed);
        Assert.Equal(
            new BoardPosition(expected_row, expected_column),
            position);
    }

    /// <summary>
    /// Confirma a rejeição de texto, quantidade e intervalo inválidos.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("1")]
    [InlineData("1 2 3")]
    [InlineData("0 1")]
    [InlineData("4 2")]
    public void try_parse_position_should_reject_invalid_input(
        string input)
    {
        bool parsed = ConsoleGameInput.try_parse_position(
            input,
            out _);

        Assert.False(parsed);
    }

    /// <summary>
    /// Confirma nova tentativa após texto inválido.
    /// </summary>
    [Fact]
    public void read_move_should_retry_after_invalid_text()
    {
        StringReader reader = new(
            "texto" + Environment.NewLine +
            "2 3" + Environment.NewLine);
        StringWriter writer = new();
        ConsoleGameInput input = new(reader, writer);
        Match match = create_match();

        BoardPosition position =
            input.read_move(match, (HumanPlayer)match.FirstPlayer);

        Assert.Equal(new BoardPosition(1, 2), position);
        Assert.Contains("Entrada inválida", writer.ToString());
    }

    /// <summary>
    /// Confirma nova tentativa quando a casa já está ocupada.
    /// </summary>
    [Fact]
    public void read_move_should_retry_after_occupied_position()
    {
        Match match = create_match();
        match.apply_move(new BoardPosition(0, 0));

        StringReader reader = new(
            "1 1" + Environment.NewLine +
            "2 2" + Environment.NewLine);
        StringWriter writer = new();
        ConsoleGameInput input = new(reader, writer);

        BoardPosition position =
            input.read_move(match, (HumanPlayer)match.FirstPlayer);

        Assert.Equal(new BoardPosition(1, 1), position);
        Assert.Contains("já está ocupada", writer.ToString());
    }

    private static Match create_match()
    {
        return new Match(
            new HumanPlayer("Pessoa", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));
    }
}
