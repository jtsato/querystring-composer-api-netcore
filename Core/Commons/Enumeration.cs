using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Core.Commons;

public class Enumeration<T> where T : Enumeration<T>
{
    public int Id { get; }
    public string Name { get; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public static IEnumerable<T> GetAll()
    {
        FieldInfo[] fieldInfos = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fieldInfos
            .Select(fieldInfo => fieldInfo.GetValue(null))
            .Cast<T>();
    }

    public static Optional<T> GetByName(string name)
    {
        if (name == null) return Optional<T>.Empty();

        FieldInfo[] fieldInfos = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return Optional<T>.Of(fieldInfos
            .Select(fieldInfo => fieldInfo.GetValue(null))
            .Cast<T>()
            .FirstOrDefault(it => it.Name.ToUpper().Equals(name.ToUpper()))
        );
    }

    public bool Is(T enumeration)
    {
        return Name.Equals(enumeration.Name);
    }

    public bool IsNot(T enumeration)
    {
        return !Is(enumeration);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
    }
}