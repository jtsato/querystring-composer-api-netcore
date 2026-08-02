using System.Collections.Generic;
using Core.Domains.QueryStrings.Models;

namespace Core.Domains.QueryStrings.Helpers;

// The item-independent half of sentence parsing: numbers and nouns matched against the full query
// structure vocabulary, plus the whitespace-noun lookup table. Computed once per search via
// SentenceParserHelper.CreateContext and reused for every item, instead of being redone from scratch
// for each one.
public sealed class SentenceParsingContext(List<WordInfo> baseWordInfos, IList<string> nounsWithWhiteSpaces)
{
    public List<WordInfo> BaseWordInfos { get; } = baseWordInfos;
    public IList<string> NounsWithWhiteSpaces { get; } = nounsWithWhiteSpaces;
}
