using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica as invariantes de <see cref="GameEvaluation"/>.
/// </summary>
public class GameEvaluationTests
{
    /// <summary>
    /// Confirma que uma vitória exige exatamente três posições.
    /// </summary>
    [Fact]
    public void constructor_should_require_three_positions_for_win()
    {
        Assert.Throws<ArgumentException>(
            () => new GameEvaluation(
                GameResult.XWins,
                [new BoardPosition(0, 0)]));
    }

    /// <summary>
    /// Confirma que empate não aceita posições vencedoras.
    /// </summary>
    [Fact]
    public void constructor_should_reject_positions_for_draw()
    {
        Assert.Throws<ArgumentException>(
            () => new GameEvaluation(
                GameResult.Draw,
                [
                    new BoardPosition(0, 0),
                    new BoardPosition(0, 1),
                    new BoardPosition(0, 2)
                ]));
    }

    /// <summary>
    /// Confirma que a coleção de posições vencedoras é copiada.
    /// </summary>
    [Fact]
    public void constructor_should_copy_winning_positions()
    {
        BoardPosition[] positions =
        [
            new BoardPosition(0, 0),
            new BoardPosition(0, 1),
            new BoardPosition(0, 2)
        ];

        GameEvaluation evaluation = new(GameResult.XWins, positions);
        positions[0] = new BoardPosition(2, 2);

        Assert.Equal(new BoardPosition(0, 0), evaluation.WinningPositions[0]);
    }
}
