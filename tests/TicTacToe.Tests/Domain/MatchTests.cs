using TicTacToe.Domain;
using Xunit;

namespace TicTacToe.Tests.Domain;

/// <summary>
/// Verifica as invariantes, os turnos e o encerramento de <see cref="Match"/>.
/// </summary>
public class MatchTests
{
    /// <summary>
    /// Confirma o estado inicial do agregado.
    /// </summary>
    [Fact]
    public void constructor_should_create_in_progress_match()
    {
        (HumanPlayer first_player, ComputerPlayer second_player) = create_players();

        Match match = new(first_player, second_player);

        Assert.Same(first_player, match.CurrentPlayer);
        Assert.Equal(GameState.InProgress, match.State);
        Assert.Equal(GameResult.None, match.Result);
        Assert.Empty(match.Moves);
        Assert.Empty(match.WinningPositions);
        Assert.Equal(0, match.Board.OccupiedCount);
    }

    /// <summary>
    /// Confirma que os participantes devem possuir símbolos diferentes.
    /// </summary>
    [Fact]
    public void constructor_should_reject_equal_symbols()
    {
        HumanPlayer first_player = new("Ana", Symbol.X);
        ComputerPlayer second_player = new("CPU", Symbol.X);

        Assert.Throws<ArgumentException>(
            () => new Match(first_player, second_player));
    }

    /// <summary>
    /// Confirma a alternância após uma jogada válida.
    /// </summary>
    [Fact]
    public void apply_move_should_alternate_current_player()
    {
        Match match = create_match();

        Move move = match.apply_move(new BoardPosition(1, 1));

        Assert.Equal(Symbol.X, move.Symbol);
        Assert.Equal(1, move.TurnNumber);
        Assert.Same(match.SecondPlayer, match.CurrentPlayer);
    }

    /// <summary>
    /// Confirma que jogadas sucessivas utilizam o símbolo do turno.
    /// </summary>
    [Fact]
    public void apply_move_should_use_current_player_symbol()
    {
        Match match = create_match();

        Move first_move = match.apply_move(new BoardPosition(0, 0));
        Move second_move = match.apply_move(new BoardPosition(1, 1));

        Assert.Equal(Symbol.X, first_move.Symbol);
        Assert.Equal(Symbol.O, second_move.Symbol);
        Assert.Equal(Symbol.X, match.Board.get_symbol(first_move.Position));
        Assert.Equal(Symbol.O, match.Board.get_symbol(second_move.Position));
    }

    /// <summary>
    /// Confirma a numeração e a ordem cronológica do histórico.
    /// </summary>
    [Fact]
    public void apply_move_should_append_ordered_history()
    {
        Match match = create_match();

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));

        Assert.Equal(3, match.Moves.Count);
        Assert.Equal([1, 2, 3], match.Moves.Select(move => move.TurnNumber));
        Assert.Equal(new BoardPosition(0, 0), match.Moves[0].Position);
        Assert.Equal(new BoardPosition(1, 0), match.Moves[1].Position);
        Assert.Equal(new BoardPosition(0, 1), match.Moves[2].Position);
    }

    /// <summary>
    /// Confirma que uma jogada inválida não altera turno nem histórico.
    /// </summary>
    [Fact]
    public void invalid_move_should_preserve_match_state()
    {
        Match match = create_match();
        BoardPosition position = new(0, 0);
        match.apply_move(position);
        Player current_player = match.CurrentPlayer;

        Assert.Throws<InvalidOperationException>(
            () => match.apply_move(position));

        Assert.Same(current_player, match.CurrentPlayer);
        Assert.Single(match.Moves);
        Assert.Equal(GameState.InProgress, match.State);
    }

    /// <summary>
    /// Confirma o encerramento por vitória.
    /// </summary>
    [Fact]
    public void apply_move_should_finish_match_after_win()
    {
        Match match = create_match();

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));
        match.apply_move(new BoardPosition(1, 1));
        match.apply_move(new BoardPosition(0, 2));

        Assert.Equal(GameState.Finished, match.State);
        Assert.Equal(GameResult.XWins, match.Result);
        Assert.Same(match.FirstPlayer, match.CurrentPlayer);
        Assert.Equal(
            [
                new BoardPosition(0, 0),
                new BoardPosition(0, 1),
                new BoardPosition(0, 2)
            ],
            match.WinningPositions);
    }

    /// <summary>
    /// Confirma o encerramento por empate.
    /// </summary>
    [Fact]
    public void apply_move_should_finish_match_after_draw()
    {
        Match match = create_match();

        BoardPosition[] positions =
        [
            new BoardPosition(0, 0),
            new BoardPosition(0, 1),
            new BoardPosition(0, 2),
            new BoardPosition(1, 1),
            new BoardPosition(1, 0),
            new BoardPosition(1, 2),
            new BoardPosition(2, 1),
            new BoardPosition(2, 0),
            new BoardPosition(2, 2)
        ];

        foreach (BoardPosition position in positions)
        {
            match.apply_move(position);
        }

        Assert.Equal(GameState.Finished, match.State);
        Assert.Equal(GameResult.Draw, match.Result);
        Assert.Empty(match.WinningPositions);
        Assert.Equal(9, match.Moves.Count);
    }

    /// <summary>
    /// Confirma que o turno não alterna após a jogada final.
    /// </summary>
    [Fact]
    public void final_move_should_keep_last_player_as_current()
    {
        Match match = create_match();

        match.apply_move(new BoardPosition(0, 0));
        match.apply_move(new BoardPosition(1, 0));
        match.apply_move(new BoardPosition(0, 1));
        match.apply_move(new BoardPosition(1, 1));
        match.apply_move(new BoardPosition(0, 2));

        Assert.Same(match.FirstPlayer, match.CurrentPlayer);
    }

    /// <summary>
    /// Confirma que não são aceitas jogadas após o encerramento.
    /// </summary>
    [Fact]
    public void apply_move_should_reject_move_after_finish()
    {
        Match match = create_finished_match();
        int move_count = match.Moves.Count;

        Assert.Throws<InvalidOperationException>(
            () => match.apply_move(new BoardPosition(2, 2)));

        Assert.Equal(move_count, match.Moves.Count);
        Assert.Equal(Symbol.Empty, match.Board.get_symbol(new BoardPosition(2, 2)));
    }

    /// <summary>
    /// Confirma a recuperação de participantes por símbolo.
    /// </summary>
    [Fact]
    public void get_player_should_return_player_by_symbol()
    {
        Match match = create_match();

        Assert.Same(match.FirstPlayer, match.get_player(Symbol.X));
        Assert.Same(match.SecondPlayer, match.get_player(Symbol.O));
    }

    /// <summary>
    /// Confirma que o símbolo vazio não identifica participante.
    /// </summary>
    [Fact]
    public void get_player_should_reject_empty_symbol()
    {
        Match match = create_match();

        Assert.Throws<ArgumentException>(
            () => match.get_player(Symbol.Empty));
    }

    private static Match create_match()
    {
        (HumanPlayer first_player, ComputerPlayer second_player) = create_players();

        return new Match(first_player, second_player);
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

    private static (HumanPlayer, ComputerPlayer) create_players()
    {
        return (
            new HumanPlayer("Ana", Symbol.X),
            new ComputerPlayer("CPU", Symbol.O));
    }
}
