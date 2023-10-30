using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Infra.MongoDB.Commons.Repository;

namespace IntegrationTest.Infra.MongoDB.Commons;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class DummyEntity : Entity
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public DateTime BirthDate { get; init; }
    public int Age { get; init; }

    public DummyEntity(int id, string name, string surname, string birthDateAsString, int age)
    {
        Id = id;
        Name = name;
        Surname = surname;
        bool isValid = DateTime.TryParse(birthDateAsString, CultureInfo.DefaultThreadCurrentCulture, out DateTime dateTime);
        BirthDate = isValid ? dateTime : DateTime.MinValue;
        Age = age;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((DummyEntity) obj);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(DummyEntity other)
    {
        return Id == other.Id
               && Name == other.Name
               && Surname == other.Surname
               && BirthDate.Equals(other.BirthDate)
               && Age == other.Age;
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Surname, BirthDate, Age);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"Id: {Id} ")
            .Append($"Name: {Name} ")
            .Append($"Surname: {Surname} ")
            .Append($"BirthDate: {BirthDate} ")
            .Append($"Age: {Age} ")
            .ToString();
    }
}