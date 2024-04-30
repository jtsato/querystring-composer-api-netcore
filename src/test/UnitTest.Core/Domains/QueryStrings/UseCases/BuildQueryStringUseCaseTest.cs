using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStrings.Commands;
using Core.Domains.QueryStrings.Gateways;
using Core.Domains.QueryStrings.Interfaces;
using Core.Domains.QueryStrings.Models;
using Core.Domains.QueryStrings.UseCases;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using Core.Exceptions;
using Moq;
using UnitTest.Core.Commons;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStrings.UseCases;

[ExcludeFromCodeCoverage]
public sealed class BuildQueryStringUseCaseTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<IGetQueryStructureByNameGateway> _getQueryStructureByNameGateway;
    private readonly Mock<IBuildQueryStringWithAiGateway> _buildQueryStringWithAiGateway;
    private readonly Mock<IGetDateTime> _getDateTime;
    private readonly IBuildQueryStringUseCase _useCase;

    public BuildQueryStringUseCaseTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _getQueryStructureByNameGateway = new Mock<IGetQueryStructureByNameGateway>(MockBehavior.Strict);
        _buildQueryStringWithAiGateway = new Mock<IBuildQueryStringWithAiGateway>(MockBehavior.Strict);
        _getDateTime = new Mock<IGetDateTime>(MockBehavior.Strict);

        ServiceResolverMocker serviceResolverMocker = new ServiceResolverMocker()
            .AddService(_getQueryStructureByNameGateway.Object)
            .AddService(_buildQueryStringWithAiGateway.Object)
            .AddService(_getDateTime.Object);

        _useCase = new BuildQueryStringUseCase(serviceResolverMocker.Object);
    }

    private bool _disposed;

    ~BuildQueryStringUseCaseTest() => Dispose(false);

    public void Dispose()
    {
        _buildQueryStringWithAiGateway.VerifyAll();
        Dispose(true);
        _outputHelper.WriteLine($"{nameof(BuildQueryStringUseCaseTest)} disposed.");
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to build query string when query structure is not found")]
    public async Task FailToBuildQueryStringWhenQueryStructureIsNotFound()
    {
        // Arrange
        _getQueryStructureByNameGateway.Setup
            (
                gateway => gateway.ExecuteAsync("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties")
            )
            .ReturnsAsync(Optional<QueryStructure>.Empty);

        // Act
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.ExecuteAsync(new BuildQueryStringCommand
            (
                clientUid: "9419357e-123b-494a-8bc3-fd17373c218b",
                queryName: "General Properties",
                searchTerms: "Casa com 3 quartos"
            )));

        // Assert
        Assert.Equal("ValidationQueryStructureByClientAndNameNotFound", exception.Message);
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", exception.Parameters[0]);
        Assert.Equal("General Properties", exception.Parameters[1]);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to build query string when query structure do not have items")]
    public async Task FailToBuildQueryStringWhenQueryStructureDoNotHaveItems()
    {
        // Arrange
        _getQueryStructureByNameGateway.Setup(gateway => gateway.ExecuteAsync("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties"))
            .ReturnsAsync(Optional<QueryStructure>.Of
                (
                    new QueryStructure
                    {
                        Id = 1,
                        ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                        Name = "General Properties",
                        Description = "General filter of properties",
                        AiSettings = new AiSettings
                        {
                            UsagePercentage = 0,
                            ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
                        },
                        CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                        UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
                    }
                )
            );

        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local));

        // Act
        Output actual = await _useCase.ExecuteAsync
        (
            new BuildQueryStringCommand("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties", "Casa com 3 quartos")
        );

        // Assert
        Assert.NotNull(actual);

        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", actual.ClientUid);
        Assert.Equal("General Properties", actual.QueryName);
        Assert.Equal("Casa com 3 quartos", actual.SearchTerms);
        Assert.Equal(string.Empty, actual.QueryString);
        Assert.False(actual.CreatedByAi);
        Assert.Equal(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local), actual.CreatedAt);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to build query string when query structure have items but no entries")]
    public async Task FailToBuildQueryStringWhenQueryStructureHaveItemsButNoEntries()
    {
        // Arrange
        _getQueryStructureByNameGateway.Setup(gateway => gateway.ExecuteAsync("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties"))
            .ReturnsAsync
            (
                Optional<QueryStructure>.Of
                (
                    new QueryStructure
                    {
                        Id = 1,
                        ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                        Name = "General Properties",
                        Description = "General filter of properties",
                        AiSettings = new AiSettings
                        {
                            UsagePercentage = 0,
                            ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
                        },
                        CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                        UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
                        Items = new List<Item>
                        {
                            new Item
                            {
                                Rank = 1, Name = "Property Type", Description = "Property Type"
                            }
                        }
                    }
                )
            );

        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local));

        // Act
        Output actual = await _useCase.ExecuteAsync
        (
            new BuildQueryStringCommand("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties", "Casa com 3 quartos")
        );

        // Assert
        Assert.NotNull(actual);

        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", actual.ClientUid);
        Assert.Equal("General Properties", actual.QueryName);
        Assert.Equal("Casa com 3 quartos", actual.SearchTerms);
        Assert.Equal(string.Empty, actual.QueryString);
        Assert.False(actual.CreatedByAi);
        Assert.False(actual.CreatedByAi);
        Assert.Equal(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local), actual.CreatedAt);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to build query string when query structure have items and entries but no keywords")]
    public async Task FailToBuildQueryStringWhenQueryStructureHaveItemsAndEntriesButNoKeywords()
    {
        // Arrange
        _getQueryStructureByNameGateway.Setup(gateway => gateway.ExecuteAsync("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties"))
            .ReturnsAsync
            (
                Optional<QueryStructure>.Of
                (
                    new QueryStructure
                    {
                        Id = 1,
                        ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                        Name = "General Properties",
                        Description = "General filter of properties",
                        AiSettings = new AiSettings
                        {
                            UsagePercentage = 0,
                            ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
                        },
                        CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                        UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
                        Items = new List<Item>
                        {
                            new Item
                            {
                                Rank = 1, Name = "Property Type", Description = "Property Type",
                                Entries = new List<Entry>
                                {
                                    new Entry {Rank = 1, Key = "TWO_STOREY_HOUSE", Exclusive = true,},
                                    new Entry {Rank = 2, Key = "APARTMENT"},
                                    new Entry {Rank = 3, Key = "HOUSE"},
                                    new Entry {Rank = 4, Key = "LAND"},
                                    new Entry {Rank = 5, Key = "COUNTRY_HOUSE"},
                                    new Entry {Rank = 6, Key = "FARM"},
                                    new Entry {Rank = 7, Key = "GARAGE"},
                                    new Entry {Rank = 8, Key = "WAREHOUSE"},
                                    new Entry {Rank = 9, Key = "OFFICE"},
                                    new Entry {Rank = 10, Key = "BUSINESS_PREMISES"},
                                    new Entry {Rank = 11, Key = "LAND_DIVISION"},
                                    new Entry {Rank = 12, Key = "OTHER"},
                                }
                            }
                        }
                    }
                )
            );

        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local));

        // Act
        Output actual = await _useCase.ExecuteAsync
        (
            new BuildQueryStringCommand("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties", "Casa com 3 quartos")
        );

        // Assert
        Assert.NotNull(actual);

        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", actual.ClientUid);
        Assert.Equal("General Properties", actual.QueryName);
        Assert.Equal("Casa com 3 quartos", actual.SearchTerms);
        Assert.Equal(string.Empty, actual.QueryString);
        Assert.False(actual.CreatedByAi);
        Assert.Equal(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local), actual.CreatedAt);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Success to build query string manually")]
    [InlineData("De R$ 100.000,00 a R$ 200.000,00", "?minPrice=100000&maxPrice=200000")]
    [InlineData("De 100 a 200 mil reais", "?minPrice=100&maxPrice=200000")]
    [InlineData("De cem mil a duzentos mil", "?minPrice=100000&maxPrice=200000")]
    [InlineData("De cento e cinquenta mil a duzentos e cinquenta mil", "?minPrice=150000&maxPrice=250000")]
    [InlineData("De 100 metros a 200 metros quadrados", "?minArea=100&maxArea=200")]
    [InlineData("De 100 a 200 m2", "?minArea=100&maxArea=200")]
    [InlineData(
        "Encontre todas as vendas de imóveis com 2 quartos e pelo menos 2 garagens no bairro pranalto, com preço de venda máximo de R$ 3.000,00",
        "?types=ALL&transaction=SALE&districts=Planalto&minBedrooms=2&minGarages=2&maxPrice=3000"
    )]
    [InlineData(
        "Imóveis entre 100 e 200 metros quadrados para alugar no centro",
        "?types=ALL&transaction=RENT&districts=Centro&minArea=100&maxArea=200"
    )]
    [InlineData(
        "Imóveis entre 100 e 200 m para alugar no centro",
        "?types=ALL&transaction=RENT&districts=Centro&minArea=100&maxArea=200"
    )]
    [InlineData(
        "Imóveis entre 100 a 200 metros quadrados para alugar no centro",
        "?types=ALL&transaction=RENT&districts=Centro&minArea=100&maxArea=200"
    )]
    [InlineData(
        "Imóveis entre 100 a 200 m para alugar no centro",
        "?types=ALL&transaction=RENT&districts=Centro&minArea=100&maxArea=200"
    )]
    [InlineData
    (
        "de R$ 3.000,00 a R$ 5.000,00",
        "?minPrice=3000&maxPrice=5000"
    )]
    [InlineData
    (
        "3.000,00 a 5.000,00",
        "?minPrice=3000&maxPrice=5000"
    )]
    [InlineData
    (
        "Mil a 2 mil reais",
        "?minPrice=1000&maxPrice=2000"
    )]
    [InlineData
    (
        "Imóveis entre 1000 a 2000 m para alugar no centro entre 1 bilhão e 500 milhões e 2 bilhões e 300 milhões e 400 mil reais",
        "?types=ALL&transaction=RENT&districts=Centro&minPrice=1500000000&maxPrice=2300400000&minArea=1000&maxArea=2000"
    )]
    [InlineData
    (
        "Imóveis entre 1000 a 2000 m para alugar no centro entre 1 bilhão e 500 milhões a 2 bilhões e 300 milhões e 400 mil reais",
        "?types=ALL&transaction=RENT&districts=Centro&minPrice=1500000000&maxPrice=2300400000&minArea=1000&maxArea=2000"
    )]
    [InlineData
    (
        "Imóveis entre 1 a 2 m para alugar no centro entre 1000 reais e 100 mil reais",
        "?types=ALL&transaction=RENT&districts=Centro&minPrice=1000&maxPrice=100000&minArea=1&maxArea=2"
    )]
    [InlineData
    (
        "Imóveis entre 3 a 4 m para alugar no centro entre 1 reals e 2 reais",
        "?types=ALL&transaction=RENT&districts=Centro&minPrice=1&maxPrice=2&minArea=3&maxArea=4"
    )]
    [InlineData
    (
        "Apartamentoo para alugar no centro com 3 quartos e 2 banheiros",
        "?types=APARTMENT&transaction=RENT&districts=Centro&minBedrooms=3&minToilets=2"
    )]
    [InlineData
    (
        "Apartamentoo para venda no fraron ou no alvorada com garage de até dez vagas e até 100000 reais",
        "?types=APARTMENT&transaction=SALE&districts=Alvorada,Fraron&minGarages=1&maxGarages=10&maxPrice=100000"
    )]
    [InlineData
    (
        "Loteamento à venda em Alvorada",
        "?types=LAND_DIVISION&transaction=SALE&districts=Alvorada"
    )]
    [InlineData
    (
        "Sobrado para alugar no centro com 3 quartos e 2 banheiros",
        "?types=TWO_STOREY_HOUSE&transaction=RENT&districts=Centro&minBedrooms=3&minToilets=2"
    )]
    [InlineData
    (
        "Apartamentuu ou Casa no centro ou no fraron para venda com no mínimo 3 quartos e dois banheiro, de até 500000 reais",
        "?types=APARTMENT,HOUSE&transaction=SALE&districts=Centro,Fraron&minBedrooms=3&minToilets=2&maxPrice=500000"
    )]
    [InlineData
    (
        "Casa no Centro, no Fraron ou no Alvorada para alugar",
        "?types=HOUSE&transaction=RENT&districts=Centro,Alvorada,Fraron"
    )]
    [InlineData
    (
        "Alugar uma casa no Centro, no Fraron ou no Alvorada, você deve.",
        "?types=HOUSE&transaction=RENT&districts=Centro,Alvorada,Fraron"
    )]
    [InlineData
    (
        "Garagem para alugar no Centro",
        "?types=GARAGE&transaction=RENT&districts=Centro"
    )]
    [InlineData
    (
        "🏠 para vender ou comprar com 3 quartos", "?types=HOUSE&transaction=SALE&minBedrooms=3"
    )]
    [InlineData
    (
        "🏘️ 📝 🏙️ 🛏️ 🚽",
        "?types=TWO_STOREY_HOUSE&transaction=RENT&districts=Centro&minBedrooms=1&minToilets=1"
    )]
    [InlineData
    (
        "🏘️ 📝 🏙️ 3 🛏️ 2 🚽",
        "?types=TWO_STOREY_HOUSE&transaction=RENT&districts=Centro&minBedrooms=3&minToilets=2"
    )]
    [InlineData
    (
        "🏘️ 📝 🏙️ 🛏️ 🛏️ 🛏️ 🚽 🚽 🚗",
        "?types=TWO_STOREY_HOUSE&transaction=RENT&districts=Centro&minBedrooms=3&minToilets=2&minGarages=1"
    )]
    [InlineData
    (
        "🏘️ 📝 🏙️ 🛏️🛏️🛏️ 🚽🚽 🚗",
        "?types=TWO_STOREY_HOUSE&transaction=RENT&districts=Centro&minBedrooms=3&minToilets=2&minGarages=1"
    )]
    [InlineData
    (
        "Casa à venda em Fraron com 4 quartos",
        "?types=HOUSE&transaction=SALE&districts=Fraron&minBedrooms=4"
    )]
    [InlineData
    (
        "Terreno para alugar em Alvorada",
        "?types=LAND&transaction=RENT&districts=Alvorada"
    )]
    [InlineData
    (
        "Sala comercial para venda no centro",
        "?types=OFFICE&transaction=SALE&districts=Centro"
    )]
    [InlineData
    (
        "Fazenda à venda",
        "?types=FARM&transaction=SALE"
    )]
    [InlineData
    (
        "🏠 no centro para aluguel com 2 banheiros",
        "?types=HOUSE&transaction=RENT&districts=Centro&minToilets=2"
    )]
    [InlineData
    (
        "Apartamento para alugar no fraron com garagem para 2 carros",
        "?types=APARTMENT&transaction=RENT&districts=Fraron&minGarages=2"
    )]
    [InlineData
    (
        "Lote no Alvorada para venda",
        "?types=LAND,LAND_DIVISION&transaction=SALE&districts=Alvorada"
    )]
    [InlineData
    (
        "Chácara à venda com 5 quartos e 3 banheiros",
        "?types=COUNTRY_HOUSE&transaction=SALE&minBedrooms=5&minToilets=3"
    )]
    [InlineData
    (
        "Barracão no Fraron para aluguel",
        "?types=WAREHOUSE&transaction=RENT&districts=Fraron"
    )]
    [InlineData
    (
        "🏬 para aluguel em Alvorada com 3 quartos",
        "?types=APARTMENT&transaction=RENT&districts=Alvorada&minBedrooms=3"
    )]
    [InlineData
    (
        "🏡 para vender em Fraron",
        "?types=COUNTRY_HOUSE&transaction=SALE&districts=Fraron"
    )]
    [InlineData
    (
        "Ponto comercial no Centro para alugar",
        "?types=BUSINESS_PREMISES&transaction=RENT&districts=Centro"
    )]
    [InlineData
    (
        "Sobrado em Alvorada para aluguel com 4 quartos",
        "?types=TWO_STOREY_HOUSE&transaction=RENT&districts=Alvorada&minBedrooms=4"
    )]
    [InlineData
    (
        "Sítio para venda com 6 quartos",
        "?types=COUNTRY_HOUSE,FARM&transaction=SALE&minBedrooms=6"
    )]
    [InlineData
    (
        "Estacionamento no Centro para alugar",
        "?types=GARAGE&transaction=RENT&districts=Centro"
    )]
    [InlineData
    (
        "Loteamento à venda em Fraron",
        "?types=LAND_DIVISION&transaction=SALE&districts=Fraron"
    )]
    [InlineData
    (
        "Escritório para alugar no centro",
        "?types=OFFICE&transaction=RENT&districts=Centro"
    )]
    [InlineData
    (
        "🚜 para venda",
        "?types=FARM&transaction=SALE"
    )]
    [InlineData
    (
        "📦 para alugar em Fraron",
        "?types=WAREHOUSE&transaction=RENT&districts=Fraron"
    )]
    [InlineData
    (
        "Armazém no Alvorada para venda",
        "?types=WAREHOUSE&transaction=SALE&districts=Alvorada"
    )]
    [InlineData
    (
        "AP no centro para alugar com vaga de garagem",
        "?types=APARTMENT&transaction=RENT&districts=Centro&minGarages=1"
    )]
    [InlineData
    (
        "Casa para alugar no La Salle com 3 quartos",
        "?types=HOUSE&transaction=RENT&districts=La Salle&minBedrooms=3"
    )]
    [InlineData
    (
        "Apartamento no Sâo Cristovao para venda até 200 mil reais",
        "?types=APARTMENT&transaction=SALE&districts=São Cristóvão&maxPrice=200000"
    )]
    [InlineData
    (
        "Apartamento no Sao Cristovão para venda até 200 mil reais",
        "?types=APARTMENT&transaction=SALE&districts=São Cristóvão&maxPrice=200000"
    )]
    [InlineData
    (
        "Apartamento no Sao Cristovao para venda até 200 mil reais",
        "?types=APARTMENT&transaction=SALE&districts=São Cristóvão&maxPrice=200000"
    )]
    [InlineData
    (
        "Nada",
        ""
    )]
    [InlineData
    (
        "Apartamento no centro com dois quartos até 10000 reais",
        "?types=APARTMENT&districts=Centro&minBedrooms=2&maxPrice=10000"
    )]
    public async Task SuccessToBuildQueryStringManually(string searchTerms, string expectedQueryString)
    {
        // Arrange

        QueryStructure queryStructure = new QueryStructure
        {
            Id = 1,
            ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
            Name = "General Properties",
            Description = "General filter of properties",
            AiSettings = new AiSettings
            {
                UsagePercentage = 100,
                ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
            },
            CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
            UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
            
            // Exclusive means that only one entry can be selected
            // For example, if the user selects "ALL" automatically the other types are covered
            // Immiscible means that the entry can not be selected with other entries
            // For example, if the user selects "GARAGE" automatically the other types are excluded
            Items = new List<Item>
            {
                new Item
                {
                    Rank = 1, Name = "types", Description = "Property Type",
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "ALL", Exclusive = true,
                            KeyWords = new List<string> {"todos", "todas", "tudo", "todes", "tudinho", "tudinha", "imóvel", "imóveis", "imovel", "imoveis", "propriedade", "propriedades"}
                        },

                        new Entry
                        {
                            Rank = 2, Key = "TWO_STOREY_HOUSE",
                            KeyWords = new List<string> {"🏘️", "sobrado", "andares"}
                        },
                        new Entry
                        {
                            Rank = 3, Key = "APARTMENT",
                            KeyWords = new List<string>
                            {
                                "🏢", "🏬", "apartamento", "apartamentos", "ap", "ape", "apt", "apzinho",
                                "apezinho", "apart", "apto", "flatinho", "flat", "kitnet", "loft",
                                "quitinete", "studio"
                            }
                        },
                        new Entry
                        {
                            Rank = 4, Key = "HOUSE",
                            KeyWords = new List<string>
                            {
                                "🏠", "🏚️", "casa", "casinha", "chalé", "edícula", "kaza", "kza", "mansão", "vivenda"
                            }
                        },
                        new Entry
                        {
                            Rank = 5, Key = "LAND",
                            KeyWords = new List<string> {"🏞️", "🌄", "terreno", "lote", "terrenos", "lotes"}
                        },
                        new Entry
                        {
                            Rank = 6, Key = "COUNTRY_HOUSE",
                            KeyWords = new List<string>
                            {
                                "🌳", "🏡", "chácara", "campo", "chacarazinha", "chacarazito", "chacarinha",
                                "chacrinha", "rural", "sítio", "sítiozinho", "sítiozito", "fazendinha",
                            }
                        },
                        new Entry
                        {
                            Rank = 7, Key = "FARM",
                            KeyWords = new List<string> {"🚜", "🌾", "🐄", "fazenda", "sítio"}
                        },
                        new Entry
                        {
                            Rank = 8, Key = "GARAGE", Immiscible = true,
                            KeyWords = new List<string>
                            {
                                "🚗", "🚘", "🅿️", "garagem", "estacionamento", "garage", "vaga", "carro",
                            }
                        },
                        new Entry
                        {
                            Rank = 9, Key = "WAREHOUSE",
                            KeyWords = new List<string>
                            {
                                "🏭", "📦", "barracão", "armazém", "armazem", "galpão", "galpao", "depósito",
                            }
                        },
                        new Entry
                        {
                            Rank = 10, Key = "OFFICE",
                            KeyWords = new List<string> {"🖥️", "🏛️", "sala", "escritório"}
                        },
                        new Entry
                        {
                            Rank = 11, Key = "BUSINESS_PREMISES",
                            KeyWords = new List<string> {"🏪", "🛍️", "ponto", "loja", "comércio"}
                        },
                        new Entry
                        {
                            Rank = 12, Key = "LAND_DIVISION",
                            KeyWords = new List<string> {"🏞️", "🌄", "loteamento", "lote"}
                        },
                        new Entry
                        {
                            Rank = 13, Key = "OTHER",
                            KeyWords = new List<string> {"❓", "❔", "outro", "outros"}
                        },
                    }
                },

                new Item
                {
                    Rank = 2, Name = "transaction", Description = "Transaction Type",
                    Entries = new List<Entry>
                    {
                        new Entry {Rank = 1, Key = "SALE", KeyWords = new List<string> {"💲", "venda", "vender"}},
                        new Entry {Rank = 2, Key = "RENT", KeyWords = new List<string> {"📝", "aluguel", "alugar"}},
                    }
                },
                new Item
                {
                    Rank = 3, Name = "districts", Description = "Districts",
                    Entries = new List<Entry>
                    {
                        new Entry { Rank = 1, Key = "Centro", KeyWords = new List<string> { "centro", "centrinho", "🏙️", "🌆", "🌃" } },
                        new Entry { Rank = 2, Key = "Aeroporto", KeyWords = new List<string> { "aeroporto", "🛫", "✈️" } },
                        new Entry { Rank = 3, Key = "Alto da Glória", KeyWords = new List<string> { "alto da glória", "alto da gloria", "alto_da_glória", "alto_da_gloria", "altodaglória", "altodagloria" } },
                        new Entry { Rank = 4, Key = "Alvorada", KeyWords = new List<string> { "alvorada" } },
                        new Entry { Rank = 5, Key = "Amadori", KeyWords = new List<string> { "amadori" } },
                        new Entry { Rank = 6, Key = "Anchieta", KeyWords = new List<string> { "anchieta" } },
                        new Entry { Rank = 7, Key = "Baixada", KeyWords = new List<string> { "baixada" } },
                        new Entry { Rank = 8, Key = "Bancários", KeyWords = new List<string> { "bancários", "bancarios" } },
                        new Entry { Rank = 9, Key = "Bela Vista", KeyWords = new List<string> { "bela vista", "belavista", "bela_vista" } },
                        new Entry { Rank = 10, Key = "Bonatto", KeyWords = new List<string> { "bonatto" } },
                        new Entry { Rank = 11, Key = "Bortot", KeyWords = new List<string> { "bortot" } },
                        new Entry { Rank = 12, Key = "Brasília", KeyWords = new List<string> { "brasília", "brasilia" } },
                        new Entry { Rank = 13, Key = "Cadorin", KeyWords = new List<string> { "cadorin" } },
                        new Entry { Rank = 14, Key = "Cristo Rei", KeyWords = new List<string> { "cristo rei", "cristo_rei", "cristorei" } },
                        new Entry { Rank = 15, Key = "Dall Ross", KeyWords = new List<string> { "dall ross", "dall_ross", "dallross" } },
                        new Entry { Rank = 16, Key = "Fraron", KeyWords = new List<string> { "fraron" } },
                        new Entry { Rank = 17, Key = "Gralha Azul", KeyWords = new List<string> { "gralha azul", "gralha_azul", "gralhaazul" } },
                        new Entry { Rank = 18, Key = "Industrial", KeyWords = new List<string> { "industrial", "industriais" } },
                        new Entry { Rank = 19, Key = "Jardim Floresta", KeyWords = new List<string> { "jardim floresta", "jardim_floresta", "jardimfloresta" } },
                        new Entry { Rank = 20, Key = "Jardim Primavera", KeyWords = new List<string> { "jardim primavera", "jardim_primavera", "jardimprimavera" } },
                        new Entry { Rank = 21, Key = "Jardim das Américas", KeyWords = new List<string> { "jardim das américas", "jardim_das_américas", "jardim das americas", "jardim_das_americas", "jardimdasaméricas", "jardimdasamericas" } },
                        new Entry { Rank = 22, Key = "La Salle", KeyWords = new List<string> { "la salle", "lasalle" } },
                        new Entry { Rank = 23, Key = "Menino Deus", KeyWords = new List<string> { "menino deus", "menino_deus", "meninodeus" } },
                        new Entry { Rank = 24, Key = "Morumbi", KeyWords = new List<string> { "morumbi" } },
                        new Entry { Rank = 25, Key = "Novo Horizonte", KeyWords = new List<string> { "novo horizonte", "novo_horizonte", "novohorizonte" } },
                        new Entry { Rank = 26, Key = "Pagnoncelli", KeyWords = new List<string> { "pagnoncelli" } },
                        new Entry { Rank = 27, Key = "Parque do Som", KeyWords = new List<string> { "parque do som", "parque_do_som", "parquedosom" } },
                        new Entry { Rank = 28, Key = "Parzianello", KeyWords = new List<string> { "parzianello" } },
                        new Entry { Rank = 29, Key = "Pinheirinho", KeyWords = new List<string> { "pinheirinho", "pinheirinhos", "pinheirinho", "pinheirinho", "pinheirinho", "pinheirinho" } },
                        new Entry { Rank = 30, Key = "Pinheiros", KeyWords = new List<string> { "pinheiros, pinheiro", "pinheiros_pinheiro", "pinheirospinheiros" } },
                        new Entry { Rank = 31, Key = "Planalto", KeyWords = new List<string> { "planalto" } },
                        new Entry { Rank = 32, Key = "Sambugaro", KeyWords = new List<string> { "sambugaro" } },
                        new Entry { Rank = 33, Key = "Santa Terezinha", KeyWords = new List<string> { "santa terezinha", "santa_terezinha", "santaterezinha", "santaterezinha", "santaterezinha", "santaterezinha" } },
                        new Entry { Rank = 34, Key = "Santo Antônio", KeyWords = new List<string> { "santo antônio", "santo antonio", "santo_antônio", "santo_antonio", "santoantônio", "santoantonio", "santo_antônio", "santo_antonio" } },
                        new Entry { Rank = 35, Key = "São Cristóvão", KeyWords = new List<string> { "são cristóvão", "sao cristovao", "são_cristóvão", "sao_cristovao", "sãocristóvão", "sãocristovao", "são_cristóvão", "são_cristovao" } },
                        new Entry { Rank = 36, Key = "São Francisco", KeyWords = new List<string> { "são francisco", "sao francisco", "são_francisco", "sao_francisco", "sãofrancisco", "saofrancisco", "são_francisco", "sao_francisco" } },
                        new Entry { Rank = 37, Key = "São João", KeyWords = new List<string> { "são joão", "sao joao", "são_joão", "sao_joao", "sãojoão", "saojoao", "são_joão", "sao_joao" } },
                        new Entry { Rank = 38, Key = "São Luiz", KeyWords = new List<string> { "são luiz", "sao luiz", "são_luiz", "sao_luiz", "sãoluiz", "saoluiz", "são_luiz", "sao_luiz" } },
                        new Entry { Rank = 39, Key = "São Roque", KeyWords = new List<string> { "são roque", "sao roque", "são_roque", "sao_roque", "sãoroque", "saoroque", "são_roque", "sao_roque" } },
                        new Entry { Rank = 40, Key = "São Vicente", KeyWords = new List<string> { "são vicente", "sao vicente", "são_vicente", "sao_vicente", "sãovicente", "saovicente", "são_vicente", "sao_vicente" } },
                        new Entry { Rank = 41, Key = "Sudoeste", KeyWords = new List<string> { "sudoeste" } },
                        new Entry { Rank = 42, Key = "Trevo da Guarany", KeyWords = new List<string> { "trevo da guarany", "trevo_da_guarany", "trevodaguarany", "trevo_da_guarany", "trevo_da_guarany", "trevodaguarany" } },
                        new Entry { Rank = 43, Key = "Veneza", KeyWords = new List<string> { "veneza" } },
                        new Entry { Rank = 44, Key = "Vila Esperança", KeyWords = new List<string> { "vila esperança", "vila esperanca", "vila_esperança", "vila_esperanca", "vilaesperança", "vilaesperanca" } },
                        new Entry { Rank = 45, Key = "Vila Isabel", KeyWords = new List<string> { "vila isabel", "vila_isabel", "vilaisabel", "vilaisabel", "vilaisabel", "vilaisabel" } }
                    },
                },
                new Item
                {
                    Rank = 4, Name = "minBedrooms", Description = "Minimum Bedrooms", IsCountable = true,
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "minBedrooms", KeyWords = new List<string> {"🛏️", "quarto", "quartos", "dormitório", "dormitórios"}
                        },
                    },
                    ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                    RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                },
                new Item
                {
                    Rank = 5, Name = "maxBedrooms", Description = "Maximum Bedrooms", IsCountable = true, WaitForConfirmationWords = true,
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "maxBedrooms", KeyWords = new List<string> {"🛏️", "quarto", "quartos", "dormitório", "dormitórios"}
                        },
                    },
                    ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    RevocationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}
                },
                new Item
                {
                    Rank = 6, Name = "minToilets", Description = "Minimum Toilets", IsCountable = true,
                    Entries = new List<Entry>
                    {
                        new Entry {Rank = 1, Key = "minToilets", KeyWords = new List<string> {"🚽", "banheiro", "banheiros", "toalete", "toalete"}},
                    },
                    ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                    RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                },
                new Item
                {
                    Rank = 7, Name = "maxToilets", Description = "Maximum Toilets", IsCountable = true, WaitForConfirmationWords = true,
                    Entries = new List<Entry>
                    {
                        new Entry {Rank = 1, Key = "maxToilets", KeyWords = new List<string> {"🚽", "banheiro", "banheiros", "toalete", "toalete"}},
                    },
                    ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    RevocationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}
                },
                new Item
                {
                    Rank = 8, Name = "minGarages", Description = "Minimum Garages", IsCountable = true,
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "minGarages", KeyWords = new List<string>
                            {
                                "🚗", "🚘", "🅿️", "garage", "garagem", "garagens", "vaga", "vagas", "carro", "carros",
                                "automóvel", "automóveis", "estacionamento", "estacionamentos"
                            }, 
                            // "Garagem no centro" means that the user wants a property with a garage in the district "Centro"
                            // "Com garagem" means that the user wants a property with at least one garage
                            // This prevents a query from being created like "types=garage&minGarages=1"
                            IncompatibleWith = new Dictionary<string, string> {["types"] = "garage"}
                        },
                    },
                    ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                    RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                },
                new Item
                {
                    Rank = 9, Name = "maxGarages", Description = "Maximum Garages", IsCountable = true, WaitForConfirmationWords = true,
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "maxGarages", KeyWords = new List<string>
                            {
                                "🚗", "🚘", "🅿️", "garage", "garagem", "garagens", "vaga", "vagas", "carro", "carros",
                                "automóvel", "automóveis", "estacionamento", "estacionamentos"
                            },
                            IncompatibleWith = new Dictionary<string, string> {["types"] = "garage"}
                        },
                    },
                    ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    RevocationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}
                },
                new Item
                {
                    Rank = 10, Name = "minPrice", Description = "Minimum Price", IsCountable = true, WaitForConfirmationWords = false,
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "minPrice", KeyWords = new List<string> {"Anonymous", "💲", "reais", "real", "R$"},
                        },
                    },
                    ConfirmationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                    RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                },
                new Item
                {
                    Rank = 11, Name = "maxPrice", Description = "Maximum Price", IsCountable = true, WaitForConfirmationWords = true,
                    Entries = new List<Entry>
                    {
                        new Entry
                        {
                            Rank = 1, Key = "maxPrice", KeyWords = new List<string> {"Anonymous", "💲", "reais", "real", "R$"},
                        },
                    },
                    ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    RevocationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                },
                new Item
                {
                    Rank = 12, Name = "minArea", Description = "Minimum Area", IsCountable = true, WaitForConfirmationWords = false,
                    Entries = new List<Entry>
                    {
                        new Entry {Rank = 1, Key = "minArea", KeyWords = new List<string> {"📐", "metros", "m", "m²", "m2"}},
                    },
                    ConfirmationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                    RevocationWords = new List<string> {"e", "abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                },
                new Item
                {
                    Rank = 13, Name = "maxArea", Description = "Maximum Area", IsCountable = true, WaitForConfirmationWords = true,
                    Entries = new List<Entry>
                    {
                        new Entry {Rank = 1, Key = "maxArea", KeyWords = new List<string> {"📐", "metros", "m", "m²", "m2"}},
                    },
                    ConfirmationWords = new List<string> {"e", "abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    RevocationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                },
            }
        };
        
        _getQueryStructureByNameGateway
            .Setup(gateway => gateway.ExecuteAsync("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties"))
            .ReturnsAsync(Optional<QueryStructure>.Of(queryStructure));
        
        _buildQueryStringWithAiGateway
            .Setup(gateway => gateway.ExecuteAsync(queryStructure, searchTerms))
            .ReturnsAsync(Optional<string>.Empty);

        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local));

        // Act
        Output actual = await _useCase.ExecuteAsync
        (
            new BuildQueryStringCommand("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties", searchTerms)
        );

        // Assert
        Assert.NotNull(actual);

        _outputHelper.WriteLine($"Search terms...........: {searchTerms}");
        _outputHelper.WriteLine($"Expected query string..: {expectedQueryString}");
        _outputHelper.WriteLine($"Generated query string.: {actual.QueryString}");

        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", actual.ClientUid);
        Assert.Equal("General Properties", actual.QueryName);
        Assert.Equal(searchTerms, actual.SearchTerms);
        Assert.Equal(expectedQueryString, actual.QueryString);
        Assert.False(actual.CreatedByAi);
        Assert.Equal(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local), actual.CreatedAt);
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "Success to build query string with AI")]
    [InlineData("De R$ 100.000,00 a R$ 200.000,00", "?minPrice=100000&maxPrice=200000")]
    public async Task SuccessToBuildQueryStringWithAi(string searchTerms, string expectedQueryString)
    {
        // Arrange
        _getQueryStructureByNameGateway
            .Setup(gateway => gateway.ExecuteAsync("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties"))
            .ReturnsAsync
            (
                Optional<QueryStructure>.Of
                (
                    new QueryStructure
                    {
                        Id = 1,
                        ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                        Name = "General Properties",
                        Description = "General filter of properties",
                        AiSettings = new AiSettings
                        {
                            UsagePercentage = 100,
                            ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
                        },
                        CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                        UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
                    }
                )
            );

        _buildQueryStringWithAiGateway
            .Setup(gateway => gateway.ExecuteAsync(
                new QueryStructure
                {
                    Id = 1,
                    ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                    Name = "General Properties",
                    Description = "General filter of properties",
                    AiSettings = new AiSettings
                    {
                        UsagePercentage = 100,
                        ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
                    },
                    CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
                    UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
                }
                , searchTerms)
            )
            .ReturnsAsync(Optional<string>.Of(expectedQueryString));

        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local));

        // Act
        Output actual = await _useCase.ExecuteAsync
        (
            new BuildQueryStringCommand("9419357e-123b-494a-8bc3-fd17373c218b", "General Properties", searchTerms)
        );

        // Assert
        Assert.NotNull(actual);

        _outputHelper.WriteLine($"Search terms...........: {searchTerms}");
        _outputHelper.WriteLine($"Expected query string..: {expectedQueryString}");
        _outputHelper.WriteLine($"Generated query string.: {actual.QueryString}");

        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", actual.ClientUid);
        Assert.Equal("General Properties", actual.QueryName);
        Assert.Equal(searchTerms, actual.SearchTerms);
        Assert.Equal(expectedQueryString, actual.QueryString);
        Assert.True(actual.CreatedByAi);
        Assert.Equal(new DateTime(2023, 10, 06, 19, 23, 32, DateTimeKind.Local), actual.CreatedAt);
    }
}
