using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains.QueryStrings.Helpers;
using Core.Domains.QueryStrings.Models;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStrings.Helpers;

public class SentenceParserHelperTest
{
    private static readonly string[] Separators = {" ", ", ", ". ", "", "?", ";", "", "-", "(", ")", "[", "]", "{", "}", "\"", "\""};
    private readonly ITestOutputHelper _outputHelper;

    private static readonly List<string> Nouns = new List<string>
    {
        "apartamento", "apartamentos", "casa", "casas", "quarto", "quartos",
        "banheiro", "banheiros", "garagem", "garagens", "sacada", "sacadas",
        "dormitório", "dormitórios", "vaga", "vagas", "loteamento", "loteamentos",
        "centro", "fraron", "alvorada", "são cristóvão", "sao cristovao", "são cristovão", "sao cristóvão",
    };

    private static readonly List<string> MinimumAdjectives = new List<string>
    {
        "acima", "desde", "maior", "mais", "min", "mínimo", "partir"
    };

    private static readonly List<string> MaximumAdjectives = new List<string>
    {
        "a", "abaixo", "antes", "até", "à", "á", "e", "inferior", "max", "máx", "máximo", "menor", "menos"
    };

    private readonly List<string> _adjectives = MinimumAdjectives.Concat(MaximumAdjectives).ToList();

    public SentenceParserHelperTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Trait("Category", "Core Business tests")]
    [Theory(DisplayName = "SuccessfullyParsedSentence")]
    [InlineData("Uma casa de dois andares no bairro Fraron com 3 quartos, um banheiro e com garagem", "casa", "Noun")]
    [InlineData("Uma casa no bairro Fraron com 3 quartos, um banheiro até 1000 reais", "Fraron", "Noun")]
    [InlineData("Uma casa no bairro Fraron com 3 quartos, um banheiro e com garagem", "3 quartos", "Quantified Noun")]
    [InlineData("Uma casa no bairro Fraron com 3 quartos, um banheiro de 2000 à 3000", "1 banheiro", "Quantified Noun")]
    [InlineData("Uma casa no bairro Fraron com 3 quartos, um banheiro e com garagem", "garagem", "Noun")]
    [InlineData("Apartamento Centro com 2 dormitórios, 1 banheiro, sacada e garagem", "Apartamento", "Noun")]
    [InlineData("Apartamento Centro com 2 dormitórios, 1 banheiro, até 1000 reais", "Centro", "Noun")]
    [InlineData("Apartamento Centro com 2 dormitórios, 1 banheiro, sacada e garagem", "2 dormitórios", "Quantified Noun")]
    [InlineData("Apartamento Centro com 2 dormitórios, 1 banheiro, de 2000 à 3000", "1 banheiro", "Quantified Noun")]
    [InlineData("Apartamento Centro com 2 dormitórios, 1 banheiro, sacada e garagem", "sacada", "Noun")]
    [InlineData("Apartamento Centro com 21 dormitórios, dez banheiros, sacada e garagem", "garagem", "Noun")]
    [InlineData("Alugar uma casa no Centro, no Fraron ou no Alvorada, você deve.", "centro", "Noun")]
    [InlineData("Alugar uma casa no Centro, de 1000 a 2000 reais, você deve.", "1000 Anonymous", "Quantified Noun")]
    [InlineData("Alugar uma casa no Centro, de 1000 a 2000 reais, você deve.", "2000 Anonymous", "Quantified Noun")]
    [InlineData("Alugar uma casa no Centro, de 1000 à 2000 reais, você deve.", "1000 Anonymous", "Quantified Noun")]
    [InlineData("Alugar uma casa no Centro, de 1000 á 2000 reais, você deve.", "2000 Anonymous", "Quantified Noun")]
    [InlineData("Alugar uma casa no Centro, de 1000 até 2000 reais, você deve.", "2000 Anonymous", "Quantified Noun")]
    public void SuccessfullyParsedSentence(string sentence, string expectedValue, string expectedType)
    {
        List<string> words = new List<string>(sentence.ToLower().Split(Separators, StringSplitOptions.RemoveEmptyEntries));
        IList<WordInfo> wordInfos = SentenceParserHelper.Parse(words, Nouns, _adjectives, new List<string>());

        _outputHelper.WriteLine($"Sentence: {sentence}");
            
        foreach (WordInfo wordInfo in wordInfos)
        {
            _outputHelper.WriteLine($"{wordInfo.Value}: {wordInfo.Type.Name}");
        }

        _outputHelper.WriteLine("--------------------------------------------------");
        _outputHelper.WriteLine($"Expected: {expectedValue}: {expectedType}");

        Assert.Contains(wordInfos, wordInfo =>
            wordInfo.Value.Equals(expectedValue, StringComparison.InvariantCultureIgnoreCase) &&
            wordInfo.Type.Name == expectedType);
    }
}