using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Suprime toda saída durante confrontos experimentais.
/// </summary>
public sealed class NullGameOutput : IGameOutput
{
    public void show_match(Match match) { }
    public void show_invalid_move(Player player, BoardPosition position, string message) { }
    public void show_result(Match match) { }
}
