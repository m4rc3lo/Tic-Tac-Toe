using TicTacToe.Domain;

namespace TicTacToe.AI;

/// <summary>
/// Seleciona jogadas por prioridades táticas e posicionais.
/// </summary>
/// <remarks>
/// A estratégia avalia, nesta ordem: vitória imediata, bloqueio de vitória
/// adversária, centro, cantos e laterais. Empates dentro de uma mesma prioridade
/// são resolvidos por <see cref="IRandomSource"/>. O tabuleiro recebido nunca é
/// modificado; todas as hipóteses são avaliadas em uma cópia interna.
/// </remarks>
public sealed class HeuristicMoveStrategy : IMoveStrategy
{
    private static readonly BoardPosition center = new(1, 1);

    private static readonly BoardPosition[] corners =
    [
        new BoardPosition(0, 0),
        new BoardPosition(0, 2),
        new BoardPosition(2, 0),
        new BoardPosition(2, 2)
    ];

    private static readonly BoardPosition[] sides =
    [
        new BoardPosition(0, 1),
        new BoardPosition(1, 0),
        new BoardPosition(1, 2),
        new BoardPosition(2, 1)
    ];

    private static readonly BoardPosition[][] winning_lines =
    [
        [new BoardPosition(0, 0), new BoardPosition(0, 1), new BoardPosition(0, 2)],
        [new BoardPosition(1, 0), new BoardPosition(1, 1), new BoardPosition(1, 2)],
        [new BoardPosition(2, 0), new BoardPosition(2, 1), new BoardPosition(2, 2)],
        [new BoardPosition(0, 0), new BoardPosition(1, 0), new BoardPosition(2, 0)],
        [new BoardPosition(0, 1), new BoardPosition(1, 1), new BoardPosition(2, 1)],
        [new BoardPosition(0, 2), new BoardPosition(1, 2), new BoardPosition(2, 2)],
        [new BoardPosition(0, 0), new BoardPosition(1, 1), new BoardPosition(2, 2)],
        [new BoardPosition(0, 2), new BoardPosition(1, 1), new BoardPosition(2, 0)]
    ];

    private readonly IRandomSource random_source;

    /// <summary>
    /// Inicializa a estratégia com gerador pseudoaleatório padrão.
    /// </summary>
    public HeuristicMoveStrategy()
        : this(new SystemRandomSource())
    {
    }

    /// <summary>
    /// Inicializa a estratégia com semente controlável.
    /// </summary>
    /// <param name="seed">Semente utilizada nos desempates.</param>
    public HeuristicMoveStrategy(int seed)
        : this(new SystemRandomSource(seed))
    {
    }

    /// <summary>
    /// Inicializa a estratégia com uma fonte pseudoaleatória injetada.
    /// </summary>
    /// <param name="random_source">Fonte utilizada nos desempates.</param>
    public HeuristicMoveStrategy(IRandomSource random_source)
    {
        ArgumentNullException.ThrowIfNull(random_source);
        this.random_source = random_source;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">
    /// Lançada quando o tabuleiro é nulo.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada quando o símbolo é vazio.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando não há casas disponíveis ou quando a fonte
    /// pseudoaleatória viola o intervalo solicitado.
    /// </exception>
    public BoardPosition choose_move(IReadOnlyBoard board, Symbol symbol)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (symbol == Symbol.Empty)
        {
            throw new ArgumentException(
                "A estratégia deve selecionar uma jogada para X ou O.",
                nameof(symbol));
        }

        IReadOnlyList<BoardPosition> available_positions =
            board.get_available_positions();

        if (available_positions.Count == 0)
        {
            throw new InvalidOperationException(
                "Não existem casas disponíveis para seleção.");
        }

        SimulationBoard simulation = new(board);

        IReadOnlyList<BoardPosition> winning_positions =
            find_winning_positions(simulation, symbol, available_positions);

        if (winning_positions.Count > 0)
        {
            return choose_candidate(winning_positions);
        }

        Symbol opponent_symbol = get_opponent_symbol(symbol);
        IReadOnlyList<BoardPosition> blocking_positions =
            find_winning_positions(
                simulation,
                opponent_symbol,
                available_positions);

        if (blocking_positions.Count > 0)
        {
            return choose_candidate(blocking_positions);
        }

        if (simulation.is_available(center))
        {
            return center;
        }

        IReadOnlyList<BoardPosition> available_corners =
            filter_available(simulation, corners);

        if (available_corners.Count > 0)
        {
            return choose_candidate(available_corners);
        }

        IReadOnlyList<BoardPosition> available_sides =
            filter_available(simulation, sides);

        if (available_sides.Count > 0)
        {
            return choose_candidate(available_sides);
        }

        throw new InvalidOperationException(
            "Não foi possível selecionar uma casa disponível.");
    }

    private IReadOnlyList<BoardPosition> find_winning_positions(
        SimulationBoard simulation,
        Symbol symbol,
        IReadOnlyList<BoardPosition> available_positions)
    {
        List<BoardPosition> candidates = [];

        foreach (BoardPosition position in available_positions)
        {
            simulation.set_symbol(position, symbol);

            if (has_winning_line(simulation, symbol))
            {
                candidates.Add(position);
            }

            simulation.set_symbol(position, Symbol.Empty);
        }

        return candidates.AsReadOnly();
    }

    private static bool has_winning_line(
        SimulationBoard simulation,
        Symbol symbol)
    {
        return winning_lines.Any(
            line => line.All(
                position => simulation.get_symbol(position) == symbol));
    }

    private static IReadOnlyList<BoardPosition> filter_available(
        SimulationBoard simulation,
        IEnumerable<BoardPosition> positions)
    {
        return positions
            .Where(simulation.is_available)
            .ToArray();
    }

    private BoardPosition choose_candidate(
        IReadOnlyList<BoardPosition> candidates)
    {
        if (candidates.Count == 1)
        {
            return candidates[0];
        }

        int selected_index = random_source.next(candidates.Count);

        if (selected_index < 0 || selected_index >= candidates.Count)
        {
            throw new InvalidOperationException(
                "O gerador pseudoaleatório retornou um índice inválido.");
        }

        return candidates[selected_index];
    }

    private static Symbol get_opponent_symbol(Symbol symbol)
    {
        return symbol == Symbol.X
            ? Symbol.O
            : Symbol.X;
    }

    /// <summary>
    /// Representa uma cópia mutável restrita ao algoritmo heurístico.
    /// </summary>
    private sealed class SimulationBoard
    {
        private readonly Symbol[,] cells =
            new Symbol[BoardPosition.BoardSize, BoardPosition.BoardSize];

        public SimulationBoard(IReadOnlyBoard board)
        {
            for (int row = 0; row < BoardPosition.BoardSize; row++)
            {
                for (int column = 0;
                     column < BoardPosition.BoardSize;
                     column++)
                {
                    BoardPosition position = new(row, column);
                    cells[row, column] = board.get_symbol(position);
                }
            }
        }

        public Symbol get_symbol(BoardPosition position)
        {
            return cells[position.Row, position.Column];
        }

        public bool is_available(BoardPosition position)
        {
            return get_symbol(position) == Symbol.Empty;
        }

        public void set_symbol(
            BoardPosition position,
            Symbol symbol)
        {
            cells[position.Row, position.Column] = symbol;
        }
    }
}
