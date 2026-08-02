using Core.Commons;

namespace Core.Domains.QueryStrings.Helpers;

public class NumberScale : Enumeration<NumberScale>
{
    public static readonly NumberScale None = new NumberScale(0, "None");
    private static readonly NumberScale Units = new NumberScale(1, "Units");
    private static readonly NumberScale Tens = new NumberScale(2, "Tens");
    private static readonly NumberScale Hundreds = new NumberScale(3, "Hundreds");
    private static readonly NumberScale Thousands = new NumberScale(4, "Thousands");
    private static readonly NumberScale Millions = new NumberScale(5, "Millions");
    private static readonly NumberScale Billions = new NumberScale(6, "Billions");

    private NumberScale(int id, string name) : base(id, name)
    {
    }
    
    public static NumberScale GetScaleByNumber(long number)
    {
        return number switch
        {
            >= 1000000000 => Billions,
            >= 1000000 => Millions,
            >= 1000 => Thousands,
            >= 100 => Hundreds,
            >= 10 => Tens,
            _ => Units
        };
    }
}