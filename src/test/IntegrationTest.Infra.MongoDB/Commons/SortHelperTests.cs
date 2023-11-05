using System.Collections.Generic;
using Core.Commons.Paging;
using Infra.MongoDB.Commons.Helpers;
using IntegrationTest.Infra.MongoDB.Commons.Dummies;
using MongoDB.Driver;
using Xunit;

namespace IntegrationTest.Infra.MongoDB.Commons;

public class SortHelperTests
{
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to get sort definition")]
    public void SuccessfulToGetSortDefinition()
    {
        // Arrange
        List<Order> orders = new List<Order>
        {
            new Order(direction: Direction.Asc, property: "Name"),
            new Order(direction: Direction.Desc, property: "Surname")
        };

        // Act
        SortDefinition<DummyEntity> sortDefinition = SortHelper.GetSortDefinitions<DummyEntity>(orders);

        // Assert
        Assert.NotNull(sortDefinition);
        Assert.IsAssignableFrom<SortDefinition<DummyEntity>>(sortDefinition);
    }
}
