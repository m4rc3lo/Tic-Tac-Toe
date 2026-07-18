using TicTacToe.Persistence.Csv;
using Xunit;

namespace TicTacToe.Tests.Persistence.Csv;

public class CsvWriterTests
{
    [Theory]
    [InlineData("texto", "texto")]
    [InlineData("a;b", "\"a;b\"")]
    [InlineData("a\"b", "\"a\"\"b\"")]
    [InlineData("a\nb", "\"a\nb\"")]
    public void escape_should_handle_special_characters(
        string value,
        string expected)
    {
        Assert.Equal(expected, CsvWriter.escape(value));
    }

    [Fact]
    public void formats_should_use_invariant_representation()
    {
        DateTimeOffset date =
            new(2026, 7, 17, 10, 30, 0, TimeSpan.FromHours(-3));

        Assert.Equal(
            "2026-07-17T13:30:00.0000000+00:00",
            CsvWriter.format_date(date));
        Assert.Equal(
            "1234.5",
            CsvWriter.format_number(1234.5));
    }
}
