using System.Collections.Generic;
using F23.StringSimilarity;

namespace Core.Domains.QueryStrings.Helpers;

internal class CharacterSubstitution : ICharacterSubstitution
{
    private readonly Dictionary<string, double> _substitutions = new Dictionary<string, double>
    {
        {"áaã", 0.5}, {"éê", 0.5}, {"íi", 0.5}, {"óoõ", 0.5}, {"úu", 0.5}, {"çc", 0.5},
        {"âa", 0.5}, {"àa", 0.5}, {"èe", 0.5}, {"üu", 0.5}, {"ñn", 0.5}, {"äa", 0.5},
        {"ëe", 0.5}, {"öo", 0.5}, {"ßs", 0.5},
    };

    public double Cost(char c1, char c2)
    {
        return _substitutions.TryGetValue($"{c1}{c2}", out double substitution) ? substitution : 1.0;
    }
}