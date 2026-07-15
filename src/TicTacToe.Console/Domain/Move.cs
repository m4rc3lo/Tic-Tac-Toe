namespace TicTacToe.Domain;

/// <summary>
/// Representa uma jogada imutável realizada em uma partida.
/// </summary>
public readonly record struct Move
{
    /// <summary>
    /// Inicializa uma nova jogada.
    /// </summary>
    /// <param name="position">Posição ocupada pela jogada.</param>
    /// <param name="symbol">Símbolo aplicado à posição.</param>
    /// <param name="turn_number">Número ordinal do turno, iniciado em um.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando o símbolo informado representa uma casa vazia.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando o número do turno é menor que um.
    /// </exception>
    public Move(
        BoardPosition position,
        Symbol symbol,
        int turn_number)
    {
        if (symbol == Symbol.Empty)
        {
            throw new ArgumentException(
                "Uma jogada deve utilizar X ou O.",
                nameof(symbol));
        }

        if (turn_number < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(turn_number),
                turn_number,
                "O número do turno deve ser maior ou igual a um.");
        }

        Position = position;
        Symbol = symbol;
        TurnNumber = turn_number;
    }

    /// <summary>
    /// Obtém a posição ocupada pela jogada.
    /// </summary>
    public BoardPosition Position { get; }

    /// <summary>
    /// Obtém o símbolo utilizado.
    /// </summary>
    public Symbol Symbol { get; }

    /// <summary>
    /// Obtém o número ordinal do turno.
    /// </summary>
    public int TurnNumber { get; }
}
