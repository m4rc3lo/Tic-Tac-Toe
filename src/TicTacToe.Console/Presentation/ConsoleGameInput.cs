using System.Globalization;
using TicTacToe.Application;
using TicTacToe.Domain;

namespace TicTacToe.Presentation;

/// <summary>
/// Obtém jogadas humanas a partir de um fluxo textual.
/// </summary>
public sealed class ConsoleGameInput : IGameInput
{
    private static readonly char[] separators =
        [' ', '\t', ',', ';'];

    private readonly TextReader reader;
    private readonly TextWriter writer;

    /// <summary>
    /// Inicializa a entrada textual.
    /// </summary>
    /// <param name="reader">Origem das linhas informadas.</param>
    /// <param name="writer">Destino dos prompts e mensagens de validação.</param>
    public ConsoleGameInput(
        TextReader reader,
        TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(writer);

        this.reader = reader;
        this.writer = writer;
    }

    /// <inheritdoc />
    public BoardPosition read_move(
        Match match,
        HumanPlayer player)
    {
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(player);

        while (true)
        {
            writer.Write(
                $"{player.Name} ({player.Symbol}), informe linha e coluna (1-3): ");

            string? input = reader.ReadLine();

            if (input is null)
            {
                throw new EndOfStreamException(
                    "A entrada foi encerrada antes da seleção de uma jogada.");
            }

            if (!try_parse_position(input, out BoardPosition position))
            {
                writer.WriteLine(
                    "Entrada inválida. Use dois números de 1 a 3.");
                continue;
            }

            if (!match.Board.is_position_available(position))
            {
                writer.WriteLine(
                    "A posição informada já está ocupada.");
                continue;
            }

            return position;
        }
    }

    /// <summary>
    /// Tenta converter coordenadas humanas de 1 a 3 para índices de 0 a 2.
    /// </summary>
    /// <param name="input">Texto recebido.</param>
    /// <param name="position">Posição convertida quando válida.</param>
    /// <returns><see langword="true"/> quando a conversão é válida.</returns>
    public static bool try_parse_position(
        string? input,
        out BoardPosition position)
    {
        position = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        string[] parts = input.Split(
            separators,
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        if (parts.Length != 2)
        {
            return false;
        }

        bool has_row = int.TryParse(
            parts[0],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int row);

        bool has_column = int.TryParse(
            parts[1],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int column);

        if (!has_row ||
            !has_column ||
            row < 1 ||
            row > BoardPosition.BoardSize ||
            column < 1 ||
            column > BoardPosition.BoardSize)
        {
            return false;
        }

        position = new BoardPosition(row - 1, column - 1);
        return true;
    }
}
