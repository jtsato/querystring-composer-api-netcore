using Core.Commons;

namespace Core.Domains.QueryStrings.Models;

public class WordInfoType : Enumeration<WordInfoType>
{
    // Common nouns (e.g., "house", "apartment", "bedroom", "toilet")
    // Proper nouns (e.g., "Brazil", "São Paulo", "Fraron")
    public static readonly WordInfoType Noun = new WordInfoType(0, "Noun");

    // Minimum Indicator (e.g., "a partir de", "desde", "mínimo", "min", "de", "no mínimo" "min")
    public static readonly WordInfoType ConfirmationIndicator = new WordInfoType(1, "Confirmation Indicator");
    
    // Maximum Indicator (e.g., "até", "antes de", "menos de", "inferior a", "abaixo de", "no máximo", "max")
    public static readonly WordInfoType RevocationIndicator = new WordInfoType(2, "Revocation Indicator");

    // Quantitative adjectives (e.g., "1", "two", "3", "four")
    public static readonly WordInfoType QuantitativeAdjective = new WordInfoType(2, "Quantitative Adjective");
    
    // Quantified Nouns (e.g., "at least 3 bedrooms", "minimum 4 toilets")
    // Quantified Nouns (e.g., "no more than 4 bedrooms", "maximum 3 toilets")
    public static readonly WordInfoType QuantifiedNoun = new WordInfoType(3, "Quantified Noun");
    
    // Any other word type not classified by the above categories
    public static readonly WordInfoType Other = new WordInfoType(4, "Other");

    private WordInfoType(int id, string name) : base(id, name)
    {
    }
}