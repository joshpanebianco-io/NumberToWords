using NumberToWords.Api;

namespace NumberToWords.Tests;

public class NumberConverterTests
{
    [Theory]
    // Example from the brief
    [InlineData("123.45", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS")]
    // Zero and singular/plural
    [InlineData("0", "ZERO DOLLARS")]
    [InlineData("0.00", "ZERO DOLLARS")]
    [InlineData("1", "ONE DOLLAR")]
    [InlineData("2", "TWO DOLLARS")]
    [InlineData("0.01", "ONE CENT")]
    [InlineData("0.02", "TWO CENTS")]
    [InlineData("1.01", "ONE DOLLAR AND ONE CENT")]
    // Teens and tens
    [InlineData("11", "ELEVEN DOLLARS")]
    [InlineData("19", "NINETEEN DOLLARS")]
    [InlineData("20", "TWENTY DOLLARS")]
    [InlineData("21", "TWENTY-ONE DOLLARS")]
    [InlineData("99", "NINETY-NINE DOLLARS")]
    // Hundreds
    [InlineData("100", "ONE HUNDRED DOLLARS")]
    [InlineData("101", "ONE HUNDRED AND ONE DOLLARS")]
    [InlineData("110", "ONE HUNDRED AND TEN DOLLARS")]
    [InlineData("999", "NINE HUNDRED AND NINETY-NINE DOLLARS")]
    // Where the AND goes above a thousand
    [InlineData("1000", "ONE THOUSAND DOLLARS")]
    [InlineData("1001", "ONE THOUSAND AND ONE DOLLARS")]
    [InlineData("1100", "ONE THOUSAND ONE HUNDRED DOLLARS")]
    [InlineData("1101", "ONE THOUSAND ONE HUNDRED AND ONE DOLLARS")]
    [InlineData("12345", "TWELVE THOUSAND THREE HUNDRED AND FORTY-FIVE DOLLARS")]
    [InlineData("100000", "ONE HUNDRED THOUSAND DOLLARS")]
    [InlineData("1000000", "ONE MILLION DOLLARS")]
    [InlineData("1000001", "ONE MILLION AND ONE DOLLARS")]
    [InlineData("1001000", "ONE MILLION ONE THOUSAND DOLLARS")]
    [InlineData("1001001", "ONE MILLION ONE THOUSAND AND ONE DOLLARS")]
    [InlineData("1000000000", "ONE BILLION DOLLARS")]
    [InlineData("1000000000000", "ONE TRILLION DOLLARS")]
    // Largest amount allowed
    [InlineData("999999999999999.99", "NINE HUNDRED AND NINETY-NINE TRILLION NINE HUNDRED AND NINETY-NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS AND NINETY-NINE CENTS")]
    // Cents formats
    [InlineData("1.5", "ONE DOLLAR AND FIFTY CENTS")]
    [InlineData("1.50", "ONE DOLLAR AND FIFTY CENTS")]
    [InlineData("1.05", "ONE DOLLAR AND FIVE CENTS")]
    [InlineData("1.00", "ONE DOLLAR")]
    [InlineData(".5", "FIFTY CENTS")]
    // Leading zeros and spaces are ignored
    [InlineData("007", "SEVEN DOLLARS")]
    [InlineData("0000000000000000001", "ONE DOLLAR")]
    [InlineData("  12  ", "TWELVE DOLLARS")]
    public void Converts_valid_amounts(string input, string expected)
    {
        bool success = NumberConverter.TryConvert(input, out string words);

        Assert.True(success);
        Assert.Equal(expected, words);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("12a")]
    [InlineData("-1")]
    [InlineData("+1")]
    [InlineData("$5")]
    [InlineData("1e5")]
    [InlineData("1,000")]
    [InlineData("1 000")]
    [InlineData(".")]
    [InlineData("1.")]
    [InlineData("1.234")]
    [InlineData("1.2.3")]
    [InlineData("١٢٣")] // Arabic-Indic digits
    [InlineData("1000000000000000")] // 16 digits, one over the limit
    public void Rejects_invalid_amounts(string? input)
    {
        bool success = NumberConverter.TryConvert(input, out string words);

        Assert.False(success);
        Assert.Equal("", words);
    }
}
