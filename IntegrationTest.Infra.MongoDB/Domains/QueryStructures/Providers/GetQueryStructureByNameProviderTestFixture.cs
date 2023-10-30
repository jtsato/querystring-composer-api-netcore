// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.QueryStructures.Models;
using IntegrationTest.Infra.MongoDB.Commons;
using MongoDB.Driver;

namespace IntegrationTest.Infra.MongoDB.Domains.QueryStructures.Providers;

public sealed class GetQueryStructureByNameProviderTestFixture : IDisposable
{
    private readonly IRepository<QueryStructureEntity> _repository;

    public GetQueryStructureByNameProviderTestFixture(Context context)
    {
        ISequenceRepository<QueryStructureSequence> sequenceRepository = context.ServiceResolver.Resolve<ISequenceRepository<QueryStructureSequence>>();
        _repository = context.ServiceResolver.Resolve<IRepository<QueryStructureEntity>>();

        FilterDefinition<QueryStructureSequence> filterDefinition = new FilterDefinitionBuilder<QueryStructureSequence>()
            .Eq(entity => entity.SequenceName, "query_structures_sequences");

        ISequence sequence = sequenceRepository.GetSequenceAndUpdate(filterDefinition).Result;

        _repository.SaveAsync
        (
            new QueryStructureEntity
            {
                Id = sequence.SequenceValue,
                ClientUid = "490f1db4-ed14-4cdc-a09f-401048951b15",
                Name = "properties-search-query-structure",
                Description = "Search query structure for properties",
                AiSettings = new AiSettingsEntity
                {
                    UsagePercentage = 100,
                    ApiKey = "ww-XyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyzXyz",
                    Model = "gpt-3.5-turbo-instruct",
                    Temperature = 0.2f,
                    MaxTokens = 1024,
                    PromptTemplate = "You are a query string builder for property search..."
                },
                CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local).ToUniversalTime(),
                UpdatedAt = new DateTime(2023, 08, 04, 18, 21, 30, DateTimeKind.Local).ToUniversalTime(),
                Items = new List<ItemEntity>
                {
                    new ItemEntity
                    {
                        Rank = 0, Name = "types", Description = "Property Type",
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 0, Key = "ALL", Exclusive = true,
                                KeyWords = new List<string> {"todos", "todas", "tudo", "todes", "tudinho", "tudinha"}
                            },
                            new EntryEntity
                            {
                                Rank = 1, Key = "APARTMENT", Exclusive = true,
                                KeyWords = new List<string>
                                {
                                    "🏢", "🏬", "apartamento", "apartamentos", "ap", "ape", "apt", "apzinho",
                                    "apezinho", "apart", "apto", "flatinho", "flat", "kitnet", "loft",
                                    "quitinete", "studio"
                                }
                            },
                            new EntryEntity
                            {
                                Rank = 2, Key = "HOUSE",
                                KeyWords = new List<string>
                                {
                                    "🏠", "🏚️", "casa", "casinha", "chalé", "edícula", "kaza", "kza", "mansão",
                                    "quitinete", "vivenda"
                                }
                            },
                            new EntryEntity
                            {
                                Rank = 3, Key = "LAND",
                                KeyWords = new List<string> {"🏞️", "🌄", "terreno", "lote"}
                            },
                            new EntryEntity
                            {
                                Rank = 4, Key = "COUNTRY_HOUSE",
                                KeyWords = new List<string>
                                {
                                    "🌳", "🏡", "chácara", "campo", "chacarazinha", "chacarazito", "chacarinha",
                                    "chacrinha", "rural", "sítio", "sítiozinho", "sítiozito", "fazendinha",
                                }
                            },
                            new EntryEntity
                            {
                                Rank = 5, Key = "FARM",
                                KeyWords = new List<string> {"🚜", "🌾", "🐄", "fazenda", "sítio"}
                            },
                            new EntryEntity
                            {
                                Rank = 6, Key = "GARAGE", Immiscible = true,
                                KeyWords = new List<string>
                                {
                                    "🚗", "🚘", "🅿️", "garagem", "estacionamento", "garage", "vaga", "carro",
                                }
                            },
                            new EntryEntity
                            {
                                Rank = 7, Key = "WAREHOUSE",
                                KeyWords = new List<string>
                                {
                                    "🏭", "📦", "barracão", "armazém", "armazem", "galpão", "galpao", "depósito",
                                }
                            },
                            new EntryEntity
                            {
                                Rank = 8, Key = "OFFICE",
                                KeyWords = new List<string> {"🖥️", "🏛️", "sala", "escritório"}
                            },
                            new EntryEntity
                            {
                                Rank = 9, Key = "BUSINESS_PREMISES",
                                KeyWords = new List<string> {"🏪", "🛍️", "ponto", "loja", "comércio"}
                            },
                            new EntryEntity
                            {
                                Rank = 10, Key = "LAND_DIVISION",
                                KeyWords = new List<string> {"🏞️", "🌄", "loteamento", "lote"}
                            },
                            new EntryEntity
                            {
                                Rank = 11, Key = "OTHER",
                                KeyWords = new List<string> {"❓", "❔", "outro", "outros"}
                            },
                        }
                    },
                    new ItemEntity
                    {
                        Rank = 1, Name = "transaction", Description = "Transaction Type",
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity {Rank = 1, Key = "SALE", KeyWords = new List<string> {"💲", "venda", "vender"}},
                            new EntryEntity {Rank = 2, Key = "RENT", KeyWords = new List<string> {"📝", "aluguel", "alugar"}},
                        }
                    },
                    new ItemEntity
                    {
                        Rank = 2, Name = "districts", Description = "Districts",
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity {Rank = 1, Key = "Centro", KeyWords = new List<string> {"centro", "centrinho", "🏙️", "🌆", "🌃"}},
                            new EntryEntity {Rank = 2, Key = "Fraron", KeyWords = new List<string> {"fraron"}},
                            new EntryEntity {Rank = 3, Key = "Alvorada", KeyWords = new List<string> {"alvorada"}},
                            new EntryEntity {Rank = 4, Key = "Planalto", KeyWords = new List<string> {"planalto"}},
                        }
                    },
                    new ItemEntity
                    {
                        Rank = 3, Name = "minBedrooms", Description = "Minimum Bedrooms", IsCountable = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "minBedrooms", KeyWords = new List<string> {"🛏️", "quarto", "quartos", "dormitório", "dormitórios"}
                            },
                        },
                        ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                        RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    },
                    new ItemEntity
                    {
                        Rank = 4, Name = "maxBedrooms", Description = "Maximum Bedrooms", IsCountable = true, WaitForConfirmationWords = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "maxBedrooms", KeyWords = new List<string> {"🛏️", "quarto", "quartos", "dormitório", "dormitórios"}
                            },
                        },
                        ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                        RevocationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}
                    },
                    new ItemEntity
                    {
                        Rank = 5, Name = "minToilets", Description = "Minimum Toilets", IsCountable = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "minToilets", KeyWords = new List<string> {"🚽", "banheiro", "banheiros", "toalete", "toalete"}
                            },
                        },
                        ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                        RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    },
                    new ItemEntity
                    {
                        Rank = 6, Name = "maxToilets", Description = "Maximum Toilets", IsCountable = true, WaitForConfirmationWords = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "maxToilets", KeyWords = new List<string> {"🚽", "banheiro", "banheiros", "toalete", "toalete"}
                            },
                        },
                        ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                        RevocationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}
                    },
                    new ItemEntity
                    {
                        Rank = 7, Name = "minGarages", Description = "Minimum Garages", IsCountable = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "minGarages", KeyWords = new List<string>
                                {
                                    "🚗", "🚘", "🅿️", "garagem", "garagens", "vaga", "vagas", "carro", "carros",
                                    "automóvel", "automóveis", "estacionamento", "estacionamentos"
                                },
                                IncompatibleWith = new Dictionary<string, string> {["types"] = "garage"}
                            },
                        },
                        ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                        RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    },
                    new ItemEntity
                    {
                        Rank = 8, Name = "maxGarages", Description = "Maximum Garages", IsCountable = true, WaitForConfirmationWords = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "maxGarages", KeyWords = new List<string>
                                {
                                    "🚗", "🚘", "🅿️", "garagem", "garagens", "vaga", "vagas", "carro", "carros",
                                    "automóvel", "automóveis", "estacionamento", "estacionamentos"
                                },
                                IncompatibleWith = new Dictionary<string, string> {["types"] = "garage"}
                            },
                        },
                        ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                        RevocationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}
                    },
                    new ItemEntity
                    {
                        Rank = 9, Name = "minPrice", Description = "Minimum Price", IsCountable = true, WaitForConfirmationWords = false,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "minPrice", KeyWords = new List<string> {"Anonymous", "💲", "reais", "real", "R$"},
                            },
                        },
                        ConfirmationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                        RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    },
                    new ItemEntity
                    {
                        Rank = 10, Name = "maxPrice", Description = "Maximum Price", IsCountable = true, WaitForConfirmationWords = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity
                            {
                                Rank = 1, Key = "maxPrice", KeyWords = new List<string> {"Anonymous", "💲", "reais", "real", "R$"},
                            },
                        },
                        ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                        RevocationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                    },
                    new ItemEntity
                    {
                        Rank = 11, Name = "minArea", Description = "Minimum Area", IsCountable = true, WaitForConfirmationWords = false,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity {Rank = 1, Key = "minArea", KeyWords = new List<string> {"📐", "metros", "m", "m²", "m2"}},
                        },
                        ConfirmationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                        RevocationWords = new List<string> {"e", "abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                    },
                    new ItemEntity
                    {
                        Rank = 12, Name = "maxArea", Description = "Maximum Area", IsCountable = true, WaitForConfirmationWords = true,
                        Entries = new List<EntryEntity>
                        {
                            new EntryEntity {Rank = 1, Key = "maxArea", KeyWords = new List<string> {"📐", "metros", "m", "m²", "m2"}},
                        },
                        ConfirmationWords = new List<string> {"e", "abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"},
                        RevocationWords = new List<string> {"entre", "acima", "desde", "maior", "mais", "min", "mínimo", "partir", "superior"},
                    }
                }
            }
        ).Wait();
    }

    ~GetQueryStructureByNameProviderTestFixture() => Dispose();

    public void Dispose()
    {
        List<FilterDefinition<QueryStructureEntity>> filterDefinitions =
            new List<FilterDefinition<QueryStructureEntity>>
            {
                new FilterDefinitionBuilder<QueryStructureEntity>().Eq(entity => entity.Name, "properties-search-query-structure"),
                new FilterDefinitionBuilder<QueryStructureEntity>().Eq(entity => entity.ClientUid, "490f1db4-ed14-4cdc-a09f-401048951b15")
            };

        _repository.DeleteManyAsync(new FilterDefinitionBuilder<QueryStructureEntity>().And(filterDefinitions)).Wait();

        GC.SuppressFinalize(this);
    }
}
