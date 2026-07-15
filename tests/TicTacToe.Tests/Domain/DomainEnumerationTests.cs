using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica os valores fundamentais das enumerações do domínio.
/// </summary>
public class DomainEnumerationTests
{
    /// <summary>
    /// Confirma que o valor padrão de <see cref="Symbol"/> representa uma casa vazia.
    /// </summary>
    [Fact]
    public void default_symbol_should_be_empty()
    {
        Symbol symbol = default;

        Assert.Equal(Symbol.Empty, symbol);
    }

    /// <summary>
    /// Confirma que o valor padrão de <see cref="GameState"/> representa partida não iniciada.
    /// </summary>
    [Fact]
    public void default_game_state_should_be_not_started()
    {
        GameState game_state = default;

        Assert.Equal(GameState.NotStarted, game_state);
    }

    /// <summary>
    /// Confirma que o valor padrão de <see cref="GameResult"/> representa ausência de resultado.
    /// </summary>
    [Fact]
    public void default_game_result_should_be_none()
    {
        GameResult game_result = default;

        Assert.Equal(GameResult.None, game_result);
    }
}
