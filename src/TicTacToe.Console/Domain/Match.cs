namespace TicTacToe.Domain;

/// <summary>
/// Representa o agregado responsável por preservar a consistência de uma partida.
/// </summary>
/// <remarks>
/// O agregado controla tabuleiro, participantes, turno atual, histórico, estado,
/// resultado e sequência vencedora. Jogadas não devem ser aplicadas diretamente
/// ao tabuleiro durante uma partida.
/// </remarks>
public sealed class Match
{
    private readonly Board board;
    private readonly List<Move> moves = [];
    private IReadOnlyList<BoardPosition> winning_positions =
        Array.Empty<BoardPosition>();

    /// <summary>
    /// Inicializa uma partida em andamento.
    /// </summary>
    /// <param name="first_player">Participante que realizará a primeira jogada.</param>
    /// <param name="second_player">Participante adversário.</param>
    /// <exception cref="ArgumentNullException">
    /// Lançada quando algum participante é nulo.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada quando os participantes controlam o mesmo símbolo.
    /// </exception>
    public Match(Player first_player, Player second_player)
    {
        ArgumentNullException.ThrowIfNull(first_player);
        ArgumentNullException.ThrowIfNull(second_player);

        if (first_player.Symbol == second_player.Symbol)
        {
            throw new ArgumentException(
                "Os participantes devem controlar símbolos diferentes.",
                nameof(second_player));
        }

        FirstPlayer = first_player;
        SecondPlayer = second_player;
        CurrentPlayer = first_player;
        board = new Board();
        Board = new BoardView(board);
        State = GameState.InProgress;
        Result = GameResult.None;
    }

    /// <summary>
    /// Obtém o tabuleiro da partida.
    /// </summary>
    public IReadOnlyBoard Board { get; }

    /// <summary>
    /// Obtém o participante que inicia a partida.
    /// </summary>
    public Player FirstPlayer { get; }

    /// <summary>
    /// Obtém o segundo participante.
    /// </summary>
    public Player SecondPlayer { get; }

    /// <summary>
    /// Obtém o participante responsável pelo turno atual.
    /// </summary>
    /// <remarks>
    /// Após o encerramento, permanece apontando para o participante que realizou
    /// a última jogada.
    /// </remarks>
    public Player CurrentPlayer { get; private set; }

    /// <summary>
    /// Obtém o histórico de jogadas em ordem cronológica.
    /// </summary>
    public IReadOnlyList<Move> Moves => moves.AsReadOnly();

    /// <summary>
    /// Obtém o estado atual da partida.
    /// </summary>
    public GameState State { get; private set; }

    /// <summary>
    /// Obtém o resultado consolidado.
    /// </summary>
    public GameResult Result { get; private set; }

    /// <summary>
    /// Obtém a sequência vencedora, quando houver.
    /// </summary>
    public IReadOnlyList<BoardPosition> WinningPositions => winning_positions;

    /// <summary>
    /// Aplica uma jogada do participante atual.
    /// </summary>
    /// <param name="position">Posição escolhida para o turno.</param>
    /// <returns>A jogada criada e registrada pelo agregado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando a partida já terminou ou a posição está ocupada.
    /// </exception>
    public Move apply_move(BoardPosition position)
    {
        if (State != GameState.InProgress)
        {
            throw new InvalidOperationException(
                "Não é possível aplicar jogadas após o encerramento da partida.");
        }

        Move move = new(
            position,
            CurrentPlayer.Symbol,
            moves.Count + 1);

        board.apply_move(move);
        moves.Add(move);

        GameEvaluation evaluation = GameRules.evaluate(board);
        Result = evaluation.Result;
        winning_positions = evaluation.WinningPositions;

        if (evaluation.IsInProgress)
        {
            CurrentPlayer = get_opponent(CurrentPlayer);
        }
        else
        {
            State = GameState.Finished;
        }

        return move;
    }

    /// <summary>
    /// Obtém o participante associado a um símbolo.
    /// </summary>
    /// <param name="symbol">Símbolo procurado.</param>
    /// <returns>Participante responsável pelo símbolo.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o símbolo é <see cref="Symbol.Empty"/>.
    /// </exception>
    public Player get_player(Symbol symbol)
    {
        return symbol switch
        {
            var value when value == FirstPlayer.Symbol => FirstPlayer,
            var value when value == SecondPlayer.Symbol => SecondPlayer,
            _ => throw new ArgumentException(
                "O símbolo deve pertencer a um participante da partida.",
                nameof(symbol))
        };
    }

    private Player get_opponent(Player player)
    {
        return ReferenceEquals(player, FirstPlayer)
            ? SecondPlayer
            : FirstPlayer;
    }
}
