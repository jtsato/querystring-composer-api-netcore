using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace IntegrationTest.Infra.MongoDB.Commons;

[ExcludeFromCodeCoverage]
[CollectionDefinition("Database collection")]
public sealed class DatabaseCollection : ICollectionFixture<Context>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}