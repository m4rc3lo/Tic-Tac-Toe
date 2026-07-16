namespace TicTacToe.Domain;

/// <summary>
/// Representa um participante de uma partida.
/// </summary>
public abstract class Player
{
    /// <summary>
    /// Inicializa um participante.
    /// </summary>
    /// <param name="name">Nome de exibição do participante.</param>
    /// <param name="symbol">Símbolo controlado pelo participante.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando o nome está vazio ou quando o símbolo é
    /// <see cref="Symbol.Empty"/>.
    /// </exception>
    protected Player(string name, Symbol symbol)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "O nome do participante não pode ser vazio.",
                nameof(name));
        }

        if (symbol == Symbol.Empty)
        {
            throw new ArgumentException(
                "Um participante deve controlar X ou O.",
                nameof(symbol));
        }

        Name = name.Trim();
        Symbol = symbol;
    }

    /// <summary>
    /// Obtém o nome de exibição.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Obtém o símbolo controlado.
    /// </summary>
    public Symbol Symbol { get; }
}
