using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Seleciona jogadas humanas pela porta de entrada e jogadas computacionais
/// pela estratégia associada ao participante.
/// </summary>
public sealed class DefaultMoveSelector : IMoveSelector
{
    private readonly IGameInput game_input;

    /// <summary>
    /// Inicializa o seletor padrão.
    /// </summary>
    /// <param name="game_input">Porta de entrada para participantes humanos.</param>
    public DefaultMoveSelector(IGameInput game_input)
    {
        ArgumentNullException.ThrowIfNull(game_input);
        this.game_input = game_input;
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">
    /// Lançada quando o tipo concreto de participante não é reconhecido.
    /// </exception>
    public BoardPosition select_move(Match match, Player player)
    {
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(player);

        return player switch
        {
            HumanPlayer human_player =>
                game_input.read_move(match, human_player),

            ComputerPlayer computer_player =>
                computer_player.choose_move(match.Board),

            _ => throw new NotSupportedException(
                $"O tipo de participante {player.GetType().Name} não é suportado.")
        };
    }
}
