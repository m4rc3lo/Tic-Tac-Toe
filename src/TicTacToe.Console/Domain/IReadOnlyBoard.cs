namespace TicTacToe.Domain;

/// <summary>
/// Expõe somente operações de consulta sobre um tabuleiro.
/// </summary>
/// <remarks>
/// O contrato impede que consumidores de uma partida apliquem ou desfaçam
/// jogadas sem passar pelo agregado <see cref="Match"/>.
/// </remarks>
public interface IReadOnlyBoard
{
    /// <summary>
    /// Obtém a quantidade de casas ocupadas.
    /// </summary>
    int OccupiedCount { get; }

    /// <summary>
    /// Obtém um valor que indica se todas as casas estão ocupadas.
    /// </summary>
    bool IsFull { get; }

    /// <summary>
    /// Obtém o símbolo armazenado em uma posição.
    /// </summary>
    /// <param name="position">Posição consultada.</param>
    /// <returns>Símbolo armazenado.</returns>
    Symbol get_symbol(BoardPosition position);

    /// <summary>
    /// Indica se uma posição está disponível.
    /// </summary>
    /// <param name="position">Posição consultada.</param>
    /// <returns><see langword="true"/> quando a casa está livre.</returns>
    bool is_position_available(BoardPosition position);

    /// <summary>
    /// Obtém uma coleção somente para leitura com as posições disponíveis.
    /// </summary>
    /// <returns>Posições livres em ordem de linha e coluna.</returns>
    IReadOnlyList<BoardPosition> get_available_positions();
}
