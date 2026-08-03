using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Core.Commons;

public class Enumeration<T> where T : Enumeration<T>
{
    private static readonly Lazy<FrozenDictionary<string, T>> ByName = new Lazy<FrozenDictionary<string, T>>(BuildByNameLookup);

    public int Id { get; }
    public string Name { get; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public static IEnumerable<T> GetAll()
    {
        return ByName.Value.Values;
    }

    public static Optional<T> GetByName(string name)
    {
        if (name == null) return Optional<T>.Empty();

        return ByName.Value.TryGetValue(name.ToUpperInvariant(), out T value)
            ? Optional<T>.Of(value)
            : Optional<T>.Empty();
    }

    private static FrozenDictionary<string, T> BuildByNameLookup()
    {
        FieldInfo[] fieldInfos = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fieldInfos
            .Select(fieldInfo => fieldInfo.GetValue(null))
            .Cast<T>()
            .ToFrozenDictionary(it => it.Name.ToUpperInvariant());
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