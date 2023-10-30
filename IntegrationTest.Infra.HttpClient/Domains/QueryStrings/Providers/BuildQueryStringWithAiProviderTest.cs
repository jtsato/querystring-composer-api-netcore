using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Models;
using Infra.HttpClient.Commons;
using Infra.HttpClient.Domains.QueryStrings.Clients;
using Infra.HttpClient.Domains.QueryStrings.Models;
using Infra.HttpClient.Domains.QueryStrings.Providers;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Integration.Infra.HttpClient.Domains.QueryStrings.Providers;

[Collection("HttpClient collection")]
public sealed class BuildQueryStringWithAiProviderTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<IOpenAiApiClient> _openAiApiClientMock;
    private readonly Mock<IGetRetryPolicy> _getRetryPolicyMock;

    public BuildQueryStringWithAiProviderTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _openAiApiClientMock = new Mock<IOpenAiApiClient>(MockBehavior.Strict);
        _getRetryPolicyMock = new Mock<IGetRetryPolicy>(MockBehavior.Strict);
    }

    private bool _disposed;

    ~BuildQueryStringWithAiProviderTest() => Dispose(false);

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        _openAiApiClientMock.VerifyAll();
        _getRetryPolicyMock.VerifyAll();
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "Infrastructure (HttpClient) Integration tests")]
    [Fact(DisplayName = "Fail to build query string with AI provider when AI response is invalid")]
    public async Task FailToBuildQueryStringWithAiProviderWhenAiResponseIsInvalid()
    {
        // Arrange
        QueryStructure queryStructure = new QueryStructure
        {
            AiSettings = new AiSettings
            {
                UsagePercentage = 100,
                ApiKey = "sk-fake-api-key",
                Model = "gpt-3.5-turbo-instruct",
                Temperature = 0.2f,
                MaxTokens = 1024,
                PromptTemplate = "You are a query string builder for property search. " +
                                 "Your task is to convert user search terms into an HTTP query string format, " +
                                 "excluding parameters that are not explicitly mentioned in the search text. " +
                                 "Here's an example with all possible parameters for reference: " +
                                 "?types=HOUSE,APARTMENT&transaction=RENT,SALE&minBedrooms=0&maxBedrooms=10" +
                                 "&minToilets=0&maxToilets=10&minGarages=0&maxGarages=10&minArea=0&maxArea=1000" +
                                 "&minBuiltArea=0&maxBuiltArea=1000&minPrice=0&maxPrice=100000. " +
                                 "Transactions (can be combined): RENT,SALE. " +
                                 "Property Types (can be combined): APARTMENT,WAREHOUSE,HOUSE,COUNTRY_HOUSE,FARM,GARAGE,LAND_DIVISION,BUSINESS_PREMISES," +
                                 "OFFICE,TWO_STOREY_HOUSE,LAND. " +
                                 "Please ensure that the district names are correctly according to the list of districts. " +
                                 "Districts list (can be combined): Aeroporto,Alto da Glória,Alvorada,Amadori,Anchieta,Baixada,Bancários,Bela Vista," +
                                 "Bonatto,Bortot,Brasília,Cadorin,Centro,Cristo Rei,Dall Ross,Fraron,Gralha Azul,Industrial,Jardim Floresta," +
                                 "Jardim Primavera,Jardim das Américas,La Salle,Menino Deus,Morumbi,Novo Horizonte,Pagnoncelli,Parque do Som," +
                                 "Parzianello,Pinheirinho,Pinheiros,Planalto,Sambugaro,Santa Terezinha,Santo Antônio,São Cristóvão," +
                                 "São Francisco,São João,São Luiz,São Roque,São Vicente,Sudoeste,Trevo da Guarany,Veneza,Vila Esperança,Vila Isabel. " +
                                 "The search term is '{{searchTerm}}'"
            }
        };

        _openAiApiClientMock
            .Setup(apiClient => apiClient.GetCompletionAsync(new CompletionRequest
            {
                Model = "gpt-3.5-turbo-instruct",
                Temperature = 0.2f,
                MaxTokens = 1024,
                Prompt = "You are a query string builder for property search. " +
                         "Your task is to convert user search terms into an HTTP query string format, " +
                         "excluding parameters that are not explicitly mentioned in the search text. " +
                         "Here's an example with all possible parameters for reference: " +
                         "?types=HOUSE,APARTMENT&transaction=RENT,SALE&minBedrooms=0&maxBedrooms=10" +
                         "&minToilets=0&maxToilets=10&minGarages=0&maxGarages=10&minArea=0&maxArea=1000" +
                         "&minBuiltArea=0&maxBuiltArea=1000&minPrice=0&maxPrice=100000. " +
                         "Transactions (can be combined): RENT,SALE. " +
                         "Property Types (can be combined): APARTMENT,WAREHOUSE,HOUSE,COUNTRY_HOUSE,FARM,GARAGE,LAND_DIVISION,BUSINESS_PREMISES," +
                         "OFFICE,TWO_STOREY_HOUSE,LAND. " +
                         "Please ensure that the district names are correctly according to the list of districts. " +
                         "Districts list (can be combined): Aeroporto,Alto da Glória,Alvorada,Amadori,Anchieta,Baixada,Bancários,Bela Vista," +
                         "Bonatto,Bortot,Brasília,Cadorin,Centro,Cristo Rei,Dall Ross,Fraron,Gralha Azul,Industrial,Jardim Floresta," +
                         "Jardim Primavera,Jardim das Américas,La Salle,Menino Deus,Morumbi,Novo Horizonte,Pagnoncelli,Parque do Som," +
                         "Parzianello,Pinheirinho,Pinheiros,Planalto,Sambugaro,Santa Terezinha,Santo Antônio,São Cristóvão," +
                         "São Francisco,São João,São Luiz,São Roque,São Vicente,Sudoeste,Trevo da Guarany,Veneza,Vila Esperança,Vila Isabel. " +
                         "The search term is 'Apartamento ou 🏠 no centro ou no fraron para venda com no mínimo 3 quartos e dois banheiro, de até 500000 reais'"
            }))
            .ReturnsAsync(new CompletionResponse
            {
                Choices = new[]
                {
                    new Choice
                    {
                        Text = @"\n\n?"
                    }
                }
            });

        _getRetryPolicyMock
            .Setup(getRetryPolicy => getRetryPolicy.Attempts)
            .Returns(1)
            .Verifiable();

        BuildQueryStringWithAiProvider provider = new BuildQueryStringWithAiProvider(_openAiApiClientMock.Object, _getRetryPolicyMock.Object);

        // Act
        Optional<string> optional = await provider.ExecuteAsync
            (queryStructure, "Apartamento ou 🏠 no centro ou no fraron para venda com no mínimo 3 quartos e dois banheiro, de até 500000 reais");

        // Assert
        Assert.False(optional.HasValue());
    }

    [Trait("Category", "Infrastructure (HttpClient) Integration tests")]
    [Fact(DisplayName = "Successful to build query string with AI provider")]
    public async Task SuccessfulToBuildQueryStringWithAiProvider()
    {
        // Arrange
        QueryStructure queryStructure = new QueryStructure
        {
            AiSettings = new AiSettings
            {
                UsagePercentage = 100,
                ApiKey = "sk-fake-api-key",
                Model = "gpt-3.5-turbo-instruct",
                Temperature = 0.2f,
                MaxTokens = 1024,
                PromptTemplate =
                    "You are a query string builder for property search. Your task is to convert user search terms into an HTTP query string format, excluding parameters that are not explicitly mentioned in the search text. Here's an example with all possible parameters for reference: ?types=HOUSE,APARTMENT&transaction=RENT,SALE&minBedrooms=0&maxBedrooms=10&minToilets=0&maxToilets=10&minGarages=0&maxGarages=10&minArea=0&maxArea=1000&minBuiltArea=0&maxBuiltArea=1000&minPrice=0&maxPrice=100000. Transactions (can be combined): RENT,SALE. Property Types (can be combined): APARTMENT,WAREHOUSE,HOUSE,COUNTRY_HOUSE,FARM,GARAGE,LAND_DIVISION,BUSINESS_PREMISES,OFFICE,TWO_STOREY_HOUSE,LAND. Please ensure that the district names are correctly according to the list of districts. Districts list (can be combined): Aeroporto,Alto da Glória,Alvorada,Amadori,Anchieta,Baixada,Bancários,Bela Vista,Bonatto,Bortot,Brasília,Cadorin,Centro,Cristo Rei,Dall Ross,Fraron,Gralha Azul,Industrial,Jardim Floresta,Jardim Primavera,Jardim das Américas,La Salle,Menino Deus,Morumbi,Novo Horizonte,Pagnoncelli,Parque do Som,Parzianello,Pinheirinho,Pinheiros,Planalto,Sambugaro,Santa Terezinha,Santo Antônio,São Cristóvão,São Francisco,São João,São Luiz,São Roque,São Vicente,Sudoeste,Trevo da Guarany,Veneza,Vila Esperança,Vila Isabel. The search term is '{{searchTerm}}'"
            }
        };

        _openAiApiClientMock
            .Setup(apiClient => apiClient.GetCompletionAsync(new CompletionRequest
            {
                Model = "gpt-3.5-turbo-instruct",
                Temperature = 0.2f,
                MaxTokens = 1024,
                Prompt =
                    @"You are a query string builder for property search. Your task is to convert user search terms into an HTTP query string format, excluding parameters that are not explicitly mentioned in the search text. Here's an example with all possible parameters for reference: ?types=HOUSE,APARTMENT&transaction=RENT,SALE&minBedrooms=0&maxBedrooms=10&minToilets=0&maxToilets=10&minGarages=0&maxGarages=10&minArea=0&maxArea=1000&minBuiltArea=0&maxBuiltArea=1000&minPrice=0&maxPrice=100000. Transactions (can be combined): RENT,SALE. Property Types (can be combined): APARTMENT,WAREHOUSE,HOUSE,COUNTRY_HOUSE,FARM,GARAGE,LAND_DIVISION,BUSINESS_PREMISES,OFFICE,TWO_STOREY_HOUSE,LAND. Please ensure that the district names are correctly according to the list of districts. Districts list (can be combined): Aeroporto,Alto da Glória,Alvorada,Amadori,Anchieta,Baixada,Bancários,Bela Vista,Bonatto,Bortot,Brasília,Cadorin,Centro,Cristo Rei,Dall Ross,Fraron,Gralha Azul,Industrial,Jardim Floresta,Jardim Primavera,Jardim das Américas,La Salle,Menino Deus,Morumbi,Novo Horizonte,Pagnoncelli,Parque do Som,Parzianello,Pinheirinho,Pinheiros,Planalto,Sambugaro,Santa Terezinha,Santo Antônio,São Cristóvão,São Francisco,São João,São Luiz,São Roque,São Vicente,Sudoeste,Trevo da Guarany,Veneza,Vila Esperança,Vila Isabel. The search term is 'Apartamento ou 🏠 no centro ou no fraron para venda com no mínimo 3 quartos e dois banheiro, de até 500000 reais'"
            }))
            .ReturnsAsync(new CompletionResponse
            {
                Choices = new[]
                {
                    new Choice
                    {
                        Text = @"\n\n?types=APARTMENT&transaction=SALE&minBedrooms=3&minToilets=2&maxPrice=500000&districts=Centro,Fraron"
                    }
                }
            });

        _getRetryPolicyMock
            .Setup(getRetryPolicy => getRetryPolicy.Attempts)
            .Returns(1)
            .Verifiable();

        BuildQueryStringWithAiProvider provider = new BuildQueryStringWithAiProvider(_openAiApiClientMock.Object, _getRetryPolicyMock.Object);

        // Act
        Optional<string> optional = await provider.ExecuteAsync
            (queryStructure, "Apartamento ou 🏠 no centro ou no fraron para venda com no mínimo 3 quartos e dois banheiro, de até 500000 reais");

        // Assert
        Assert.True(optional.HasValue());

        string actual = optional.GetValue();

        _outputHelper.WriteLine(actual);

        Assert.Equal("?types=APARTMENT&transaction=SALE&minBedrooms=3&minToilets=2&maxPrice=500000&districts=Centro,Fraron", actual);
    }
}