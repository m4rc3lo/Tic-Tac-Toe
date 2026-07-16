using TicTacToe.Domain;

namespace TicTacToe.AI;

/// <summary>
/// Seleciona uniformemente uma das casas disponíveis.
/// </summary>
/// <remarks>
/// Esta classe implementa o padrão Strategy. A dependência de aleatoriedade é
/// injetada por <see cref="IRandomSource"/>, permitindo substituir o gerador em
/// testes e controlar sementes em experimentos.
/// </remarks>
public sealed class RandomMoveStrategy : IMoveStrategy
{
    private readonly IRandomSource random_source;

    /// <summary>
    /// Inicializa a estratégia com gerador padrão.
    /// </summary>
    public RandomMoveStrategy()
        : this(new SystemRandomSource())
    {
    }

    /// <summary>
    /// Inicializa a estratégia com semente controlável.
    /// </summary>
    /// <param name="seed">Semente pseudoaleatória.</param>
    public RandomMoveStrategy(int seed)
        : this(new SystemRandomSource(seed))
    {
    }

    /// <summary>
    /// Inicializa a estratégia com um gerador injetado.
    /// </summary>
    /// <param name="random_source">Fonte de números pseudoaleatórios.</param>
    public RandomMoveStrategy(IRandomSource random_source)
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
    /// Lançada quando o tabuleiro não possui casas livres.
    /// </exception>
    public BoardPosition choose_move(Board board, Symbol symbol)
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

        int selected_index = random_source.next(available_positions.Count);

        if (selected_index < 0 || selected_index >= available_positions.Count)
        {
            throw new InvalidOperationException(
                "O gerador pseudoaleatório retornou um índice inválido.");
        }

        return available_positions[selected_index];
    }
}
