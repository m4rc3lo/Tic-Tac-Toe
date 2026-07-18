using System.Globalization;
using System.Text;

namespace TicTacToe.Persistence.Csv;

/// <summary>
/// Escreve documentos CSV UTF-8 separados por ponto e vírgula.
/// </summary>
public sealed class CsvWriter
{
    private const char Separator = ';';

    /// <summary>
    /// Escreve cabeçalho e linhas em arquivo UTF-8 sem BOM.
    /// </summary>
    public void write(
        string path,
        IReadOnlyList<string> header,
        IEnumerable<IReadOnlyList<string?>> rows)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(header);
        ArgumentNullException.ThrowIfNull(rows);

        if (header.Count == 0)
        {
            throw new ArgumentException(
                "O cabeçalho deve conter ao menos uma coluna.",
                nameof(header));
        }

        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string temporary_path =
            $"{path}.tmp-{Guid.NewGuid():N}";

        try
        {
            using (StreamWriter writer = new(
                       temporary_path,
                       append: false,
                       new UTF8Encoding(
                           encoderShouldEmitUTF8Identifier: false)))
            {
                write_row(writer, header);

                foreach (IReadOnlyList<string?> row in rows)
                {
                    if (row.Count != header.Count)
                    {
                        throw new InvalidOperationException(
                            "Todas as linhas devem possuir a mesma quantidade de colunas do cabeçalho.");
                    }

                    write_row(writer, row);
                }
            }

            File.Move(
                temporary_path,
                path,
                overwrite: true);
        }
        finally
        {
            if (File.Exists(temporary_path))
            {
                File.Delete(temporary_path);
            }
        }
    }

    /// <summary>
    /// Formata um campo conforme regras de escape CSV.
    /// </summary>
    public static string escape(string? value)
    {
        string normalized = value ?? string.Empty;

        bool requires_quotes =
            normalized.Contains(Separator) ||
            normalized.Contains('"') ||
            normalized.Contains('\r') ||
            normalized.Contains('\n');

        if (!requires_quotes)
        {
            return normalized;
        }

        return $"\"{normalized.Replace("\"", "\"\"")}\"";
    }

    /// <summary>
    /// Formata datas de modo estável em ISO 8601.
    /// </summary>
    public static string format_date(DateTimeOffset value)
    {
        return value.ToUniversalTime()
            .ToString("O", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formata números inteiros com cultura invariável.
    /// </summary>
    public static string format_number(long value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formata números de ponto flutuante com cultura invariável.
    /// </summary>
    public static string format_number(double value)
    {
        return value.ToString(
            "0.################",
            CultureInfo.InvariantCulture);
    }

    private static void write_row(
        TextWriter writer,
        IReadOnlyList<string?> values)
    {
        writer.WriteLine(
            string.Join(
                Separator,
                values.Select(escape)));
    }
}
