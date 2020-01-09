using VerticalMade.NumbersToWords;
using Xunit;

namespace VerticalMade.Test.NumbersToWords
{
    public class DollarAmountExtensionsTests
    {
        [Theory]
        [InlineData(0, "zero dollars")]
        [InlineData(0.01, "zero dollars and one cent")]
        [InlineData(0.02, "zero dollars and two cents")]
        [InlineData(0.99, "zero dollars and ninety-nine cents")]
        [InlineData(1, "one dollar")]
        [InlineData(1.01, "one dollar and one cent")]
        [InlineData(1.02, "one dollar and two cents")]
        [InlineData(2, "two dollars")]
        [InlineData(2.01, "two dollars and one cent")]
        [InlineData(2.02, "two dollars and two cents")]
        [InlineData(10, "ten dollars")]
        [InlineData(11, "eleven dollars")]
        [InlineData(19, "nineteen dollars")]
        [InlineData(20, "twenty dollars")]
        [InlineData(23, "twenty-three dollars")]
        [InlineData(30, "thirty dollars")]
        [InlineData(34, "thirty-four dollars")]
        [InlineData(40, "forty dollars")]
        [InlineData(95, "ninety-five dollars")]
        [InlineData(100, "one hundred dollars")]
        [InlineData(106, "one hundred and six dollars")]
        [InlineData(112, "one hundred and twelve dollars")]
        [InlineData(250.60, "two hundred and fifty dollars and sixty cents")]
        [InlineData(375.89, "three hundred and seventy-five dollars and eighty-nine cents")]
        [InlineData(1_000, "one thousand dollars")]
        [InlineData(1_007, "one thousand and seven dollars")]
        [InlineData(1_849, "one thousand, eight hundred and forty-nine dollars")]
        [InlineData(12_000, "twelve thousand dollars")]
        [InlineData(13_000_000, "thirteen million dollars")]
        [InlineData(14_000_001, "fourteen million and one dollars")]
        [InlineData(888_088, "eight hundred eighty-eight thousand and eighty-eight dollars")]
        [InlineData(215_000_016_017.18, "two hundred fifteen billion, sixteen thousand and seventeen dollars and eighteen cents")]
        [InlineData(19_000_000_001.01, "nineteen billion and one dollars and one cent")]
        [InlineData(-1000.01, "negative one thousand dollars and one cent")]
        public void ToDollarsAndCents_ShouldConvertAppropriately(
            decimal input, string expected
        ) => Assert.Equal(expected, ((decimal)input).ToDollarsAndCents());
    }
}
