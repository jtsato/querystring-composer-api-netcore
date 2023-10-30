using System.Threading;
using EntryPoint.WebApi.Commons.Filters;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Filters;

[Collection("WebApi Collection [NoContext]")]
public sealed class GetLanguageActionFilterAttributeTest
{
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Successful to handle multiple cultures")]
    [InlineData("pt-br", "pt-BR")]
    [InlineData("en-us", "en-US")]
    [InlineData("pt-us", "en-US")]
    [InlineData("", "en-US")]
    [InlineData(null, "en-US")]
    public void SuccessfulToHandleMultipleCultures(string language, string expected)
    {
        GetLanguageActionFilterAttribute.SetupLanguage(language);
        Assert.Equal(Thread.CurrentThread.CurrentCulture.Name, expected);
    }
}