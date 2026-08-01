using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Domains.QueryStructures.Models;

namespace UnitTest.Core.Domains.QueryStrings.UseCases;

// The "General Properties" query structure shared by the golden suite and the characterization suite.
// Exclusive means that only one entry can be selected.
// For example, if the user selects "ALL" automatically the other types are covered.
// Immiscible means that the entry can not be selected with other entries.
// For example, if the user selects "GARAGE" automatically the other types are excluded.
[ExcludeFromCodeCoverage]
public static class GeneralPropertiesFixture
{
    public const string ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b";
    public const string QueryName = "General Properties";

    public static QueryStructure Build()
    {
        return new QueryStructure
        {
            Id = 1,
            ClientUid = ClientUid,
            Name = QueryName,
            Description = "General filter of properties",
            AiSettings = new AiSettings
            {
                UsagePercentage = 100,
                ApiKey = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"
            },
            CreatedAt = new DateTime(2023, 08, 04, 17, 21, 30, DateTimeKind.Local),
            UpdatedAt = new DateTime(2024, 09, 05, 18, 22, 31, DateTimeKind.Local),
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
                            KeyWords = new List<string> {"🏘️", "sobrado", "andares"},
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
                                "🏠", "🏚️", "casa", "casinha", "chalé", "edícula", "kaza", "kza", "mansão", "vivenda",
                            },
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
                            Rank = 10, Key = "OFFICE", Immiscible = true,
                            KeyWords = new List<string> {"🖥️", "🏛️", "sala", "sala comercial", "sala_comercial", "escritório", "escritorio"}
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
                        new Entry {Rank = 1, Key = "SALE", KeyWords = new List<string> {"💲", "venda", "vender", "compra", "comprar"}},
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
                        new Entry { Rank = 19, Key = "Jardim Floresta", KeyWords = new List<string> { "jardim", "jardim floresta", "jardim_floresta", "jardimfloresta" } },
                        new Entry { Rank = 20, Key = "Jardim Primavera", KeyWords = new List<string> { "jardim", "jardim primavera", "jardim_primavera", "jardimprimavera" } },
                        new Entry { Rank = 21, Key = "Jardim das Américas", KeyWords = new List<string> { "jardim", "jardim das américas", "jardim_das_américas", "jardim das americas", "jardim_das_americas", "jardimdasaméricas", "jardimdasamericas" } },
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
                    RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo", "por", "por_menos", "pr_menos_de"},
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
                    ConfirmationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo", "por", "por_menos", "pr_menos_de"},
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
    }
}
