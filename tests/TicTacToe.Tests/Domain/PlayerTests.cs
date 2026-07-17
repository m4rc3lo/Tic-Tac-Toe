using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica as invariantes dos participantes.
/// </summary>
public class PlayerTests
{
    /// <summary>
    /// Confirma a criação de uma pessoa participante.
    /// </summary>
    [Fact]
    public void human_player_should_preserve_name_and_symbol()
    {
        HumanPlayer player = new("Ana", Symbol.X);

        Assert.Equal("Ana", player.Name);
        Assert.Equal(Symbol.X, player.Symbol);
    }

    /// <summary>
    /// Confirma a normalização de espaços externos no nome.
    /// </summary>
    [Fact]
    public void player_should_trim_name()
    {
        ComputerPlayer player = new("  CPU  ", Symbol.O);

        Assert.Equal("CPU", player.Name);
    }

    /// <summary>
    /// Confirma que nomes vazios são rejeitados.
    /// </summary>
    [Fact]
    public void player_should_reject_empty_name()
    {
        Assert.Throws<ArgumentException>(
            () => new HumanPlayer(" ", Symbol.X));
    }

    /// <summary>
    /// Confirma que participantes não podem controlar o símbolo vazio.
    /// </summary>
    [Fact]
    public void player_should_reject_empty_symbol()
    {
        Assert.Throws<ArgumentException>(
            () => new ComputerPlayer("CPU", Symbol.Empty));
    }
}
