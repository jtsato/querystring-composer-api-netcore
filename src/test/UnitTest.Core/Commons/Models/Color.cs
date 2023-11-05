using Core.Commons;

namespace UnitTest.Core.Commons.Models;

public sealed class Color : Enumeration<Color>
{
    public static readonly Color Blue = new Color(1, "BLUE");
    public static readonly Color Green = new Color(2, "GREEN");
    public static readonly Color Yellow = new Color(3, "YELLOW");
    public static readonly Color Red = new Color(4, "RED");
    public static readonly Color Black = new Color(5, "BLACK");

    private Color(int id, string name) : base(id, name)
    {
    }
}