using System.Collections.Generic;

namespace Core.Domains.QueryStrings.Helpers;

public static class NumberConverterHelper
{
    private static readonly Dictionary<string, long> WordToBasicNumberMapping = new Dictionary<string, long>
    {
        ["um"] = 1, ["dois"] = 2, ["três"] = 3, ["quatro"] = 4, ["cinco"] = 5, ["seis"] = 6, ["sete"] = 7, ["oito"] = 8, ["nove"] = 9, ["dez"] = 10,
        ["onze"] = 11, ["doze"] = 12, ["treze"] = 13, ["catorze"] = 14, ["quinze"] = 15, ["dezesseis"] = 16, ["dezessete"] = 17, ["dezoito"] = 18, ["dezenove"] = 19,
        ["vinte"] = 20, ["trinta"] = 30, ["quarenta"] = 40, ["cinquenta"] = 50, ["sessenta"] = 60, ["setenta"] = 70, ["oitenta"] = 80, ["noventa"] = 90,
        ["cem"] = 100, ["cento"] = 100, ["duzentos"] = 200, ["trezentos"] = 300, ["quatrocentos"] = 400, ["quinhentos"] = 500, ["seiscentos"] = 600,
        ["setecentos"] = 700, ["oitocentos"] = 800, ["novecentos"] = 900
    };

    private static readonly Dictionary<string, long> WordToMultiplierMapping = new Dictionary<string, long>
    {
        ["bilhão"] = 1000000000, ["bilhões"] = 1000000000, ["bi"] = 1000000000, 
        ["milhão"] = 1000000, ["milhões"] = 1000000, ["mi"] = 1000000,
        ["mil"] = 1000, ["k"] = 1000, ["milhar"] = 1000, ["milhares"] = 1000,
    };

    public static long ConvertWordsToNumber(string words)
    {
        string[] wordArray = words.ToLowerInvariant().Split(' ');

        long totalValue = 0;
        long currentValue = 0;

        foreach (string word in wordArray)
        {
            if (IsBasicNumber(word, out long basicNumber))
            {
                currentValue += basicNumber;
                continue;
            }

            if (currentValue == 0 && word == "k") continue;
                
            if (IsMultiplier(word, out long multiplier))
            {
                totalValue += (currentValue == 0 ? 1 : currentValue) * multiplier;
                currentValue = 0;
                continue;
            }

            if (IsCompositeNumber(word, out long compositeValue))
            {
                totalValue += compositeValue;
                currentValue = 0;
                continue;
            }

            if (long.TryParse(word, out long numberValue))
            {
                currentValue += numberValue;
            }
        }

        return totalValue + currentValue;
    }

    private static bool IsBasicNumber(string word, out long value)
    {
        return WordToBasicNumberMapping.TryGetValue(word, out value);
    }

    private static bool IsMultiplier(string word, out long value)
    {
        return WordToMultiplierMapping.TryGetValue(word, out value);
    }

    private static bool IsCompositeNumber(string word, out long value)
    {
        string cleanWord = word.Replace(".", string.Empty);
        foreach (string key in WordToMultiplierMapping.Keys)
        {
            if (!cleanWord.Contains(key)) continue;
            string numberPart = cleanWord.Replace(key, string.Empty);
            if (!long.TryParse(numberPart, out long numberValue) || !WordToMultiplierMapping.TryGetValue(key, out long multiplier)) continue;
            value = numberValue * multiplier;
            return true;
        }

        value = 0;
        return false;
    }
}