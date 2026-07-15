namespace TicTacToe.Domain;

/// <summary>
/// Representa uma posição válida em um tabuleiro de jogo da velha 3 × 3.
/// </summary>
/// <remarks>
/// As coordenadas utilizam índice iniciado em zero. Assim, linhas e colunas
/// válidas pertencem ao intervalo de zero a dois.
/// </remarks>
public readonly record struct BoardPosition
{
    /// <summary>
    /// Quantidade fixa de linhas e colunas do tabuleiro.
    /// </summary>
    public const int BoardSize = 3;

    /// <summary>
    /// Inicializa uma nova posição do tabuleiro.
    /// </summary>
    /// <param name="row">Índice da linha, entre zero e dois.</param>
    /// <param name="column">Índice da coluna, entre zero e dois.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando a linha ou a coluna está fora dos limites do tabuleiro.
    /// </exception>
    public BoardPosition(int row, int column)
    {
        validate_coordinate(row, nameof(row));
        validate_coordinate(column, nameof(column));

        Row = row;
        Column = column;
    }

    /// <summary>
    /// Obtém o índice da linha.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Obtém o índice da coluna.
    /// </summary>
    public int Column { get; }

    private static void validate_coordinate(int coordinate, string parameter_name)
    {
        if (coordinate < 0 || coordinate >= BoardSize)
        {
            throw new ArgumentOutOfRangeException(
                parameter_name,
                coordinate,
                $"A coordenada deve estar entre 0 e {BoardSize - 1}.");
        }
    }
}
