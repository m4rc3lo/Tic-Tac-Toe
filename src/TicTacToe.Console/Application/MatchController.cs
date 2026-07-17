using TicTacToe.Domain;

namespace TicTacToe.Application;

/// <summary>
/// Coordena o fluxo mínimo de uma partida sem conhecer Console ou mecanismos
/// concretos de entrada e saída.
/// </summary>
/// <remarks>
/// O controlador não implementa regras de domínio. Ele solicita posições,
/// delega sua aplicação ao agregado <see cref="Match"/> e comunica eventos pelas
/// portas de saída.
/// </remarks>
public sealed class MatchController
{
    private readonly IMoveSelector move_selector;
    private readonly IGameOutput game_output;

    /// <summary>
    /// Inicializa o controlador.
    /// </summary>
    /// <param name="move_selector">Porta utilizada para selecionar jogadas.</param>
    /// <param name="game_output">Porta utilizada para apresentar a partida.</param>
    public MatchController(
        IMoveSelector move_selector,
        IGameOutput game_output)
    {
        ArgumentNullException.ThrowIfNull(move_selector);
        ArgumentNullException.ThrowIfNull(game_output);

        this.move_selector = move_selector;
        this.game_output = game_output;
    }

    /// <summary>
    /// Executa a partida até vitória ou empate.
    /// </summary>
    /// <param name="match">Agregado que será coordenado.</param>
    /// <returns>A mesma instância, já encerrada.</returns>
    /// <exception cref="ArgumentNullException">
    /// Lançada quando a partida é nula.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando a partida recebida já está encerrada.
    /// </exception>
    public Match run(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);

        if (match.State != GameState.InProgress)
        {
            throw new InvalidOperationException(
                "O controlador exige uma partida em andamento.");
        }

        game_output.show_match(match);

        while (match.State == GameState.InProgress)
        {
            Player current_player = match.CurrentPlayer;
            BoardPosition position =
                move_selector.select_move(match, current_player);

            try
            {
                match.apply_move(position);
            }
            catch (InvalidOperationException exception)
            {
                game_output.show_invalid_move(
                    current_player,
                    position,
                    exception.Message);

                continue;
            }

            game_output.show_match(match);
        }

        game_output.show_result(match);

        return match;
    }
}
