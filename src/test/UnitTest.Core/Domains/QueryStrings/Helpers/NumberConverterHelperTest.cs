using Core.Domains.QueryStrings.Helpers;
using Xunit;

namespace UnitTest.Core.Domains.QueryStrings.Helpers;

public class NumberConverterHelperTest
{
    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "SuccessToConvertWordsToNumber")]
    [InlineData("dez mil", 10000)]
    [InlineData("10k", 10000)]
    [InlineData("oito mil", 8000)]
    [InlineData("quinhentos mil", 500000)]
    [InlineData("500k", 500000)]
    [InlineData("cem milhões", 100000000)]
    [InlineData("um", 1)]
    [InlineData("dois", 2)]
    [InlineData("três", 3)]
    [InlineData("novecentos", 900)]
    [InlineData("oitocentos", 800)]
    [InlineData("milhão", 1000000)]
    [InlineData("mil", 1000)]
    [InlineData("vinte e um", 21)]
    [InlineData("duzentos e dez", 210)]
    [InlineData("duzentos e dez mil", 210000)]
    [InlineData("dois milhões e dez", 2000010)]
    [InlineData("500 k", 500000)]
    [InlineData("três milhões e duzentos e dez mil", 3210000)]
    [InlineData("trezentos k", 300000)]
    [InlineData("trezentos milhar", 300000)]
    [InlineData("um milhar", 1000)]
    [InlineData("um milhão e um", 1000001)]
    [InlineData("setecentos e cinquenta e cinco mil", 755000)]
    [InlineData("setecentos e cinquenta e cinco mil e quinhentos", 755500)]
    [InlineData("abc", 0)]
    [InlineData("abc k", 0)]
    [InlineData("xyz milhão", 1000000)]
    [InlineData("vinte e um milhões", 21000000)]
    [InlineData("3ky", 0)]
    [InlineData("3yk", 0)]
    public void SuccessToConvertWordsToNumber(string input, long expected)
    {
        long result = NumberConverterHelper.ConvertWordsToNumber(input);
        Assert.Equal(expected, result);
    }
}