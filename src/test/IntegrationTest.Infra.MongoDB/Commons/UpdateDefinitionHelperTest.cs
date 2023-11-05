using System.Collections.Generic;
using Infra.MongoDB.Commons.Helpers;
using IntegrationTest.Infra.MongoDB.Commons.Dummies;
using MongoDB.Driver;
using Xunit;

namespace IntegrationTest.Infra.MongoDB.Commons;

public class UpdateDefinitionHelperTest
{
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add up definition if value has changed")]
    public void SuccessfulToAddUpDefinitionIfValueHasChanged()
    {
        // Arrange
        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfValueHasChanged(ref updateDefinitions, "Name", "John", "Kim");

        // Assert
        Assert.Single(updateDefinitions);
    }
    
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add up definition if value was null")]
    public void SuccessfulToAddUpDefinitionIfValueWasNull()
    {
        // Arrange
        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfValueHasChanged(ref updateDefinitions, "Name", null, "Kim");

        // Assert
        Assert.Single(updateDefinitions);
    }
        
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add up definition if value is null")]
    public void SuccessfulToAddUpDefinitionIfValueIsNull()
    {
        // Arrange
        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfValueHasChanged(ref updateDefinitions, "Name", "Kill", null);

        // Assert
        Assert.Single(updateDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Fail to add up definition if value has not changed")]
    public void FailToAddUpDefinitionIfValueHasNotChanged()
    {
        // Arrange
        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfValueHasChanged(ref updateDefinitions, "Name", "John", "John");

        // Assert
        Assert.Empty(updateDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add multiple up definitions if value has changed")]
    public void SuccessfulToAddMultipleUpDefinitionsIfValueHasChanged()
    {
        // Arrange
        List<DummyEntity> currentDummies = new List<DummyEntity>
        {
            new DummyEntity(1, "John", "Smith", "1980-04-23 00:00:01", 29),
            new DummyEntity(2, "Kim", "Smith", "1981-07-25 00:00:01", 31),
        };

        List<DummyEntity> newDummies = new List<DummyEntity>
        {
            new DummyEntity(3, "Tiebout", "Moskovitz", "1981-07-07 00:00:01", 29),
            new DummyEntity(4, "Marlane", "Moskovitz", "1984-10-10 00:00:01", 31),
        };

        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfItemsHasChanged(ref updateDefinitions, "dummies", currentDummies, newDummies);

        // Assert
        Assert.Single(updateDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Fail to add multiple up definitions if value has not changed")]
    public void FailToAddMultipleUpDefinitionsIfValueHasNotChanged()
    {
        // Arrange
        List<DummyEntity> currentDummies = new List<DummyEntity>
        {
            new DummyEntity(1, "John", "Smith", "1980-04-23 00:00:01", 29),
            new DummyEntity(2, "Kim", "Smith", "1981-07-25 00:00:01", 31),
        };

        List<DummyEntity> newDummies = new List<DummyEntity>
        {
            new DummyEntity(1, "John", "Smith", "1980-04-23 00:00:01", 29),
            new DummyEntity(2, "Kim", "Smith", "1981-07-25 00:00:01", 31),
        };

        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfItemsHasChanged(ref updateDefinitions, "dummies", currentDummies, newDummies);

        // Assert
        Assert.Empty(updateDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Fail to add multiple up definitions if values are null")]
    public void FailToAddMultipleUpDefinitionsIfValuesAreNull()
    {
        // Arrange
        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfItemsHasChanged(ref updateDefinitions, "dummies", (List<DummyEntity>) null, null);

        // Assert
        Assert.Empty(updateDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add multiple up definitions if current value is null")]
    public void SuccessfulToAddMultipleUpDefinitionsIfCurrentValueIsNull()
    {
        // Arrange
        List<DummyEntity> newDummies = new List<DummyEntity>
        {
            new DummyEntity(1, "John", "Smith", "1980-04-23 00:00:01", 29),
            new DummyEntity(2, "Kim", "Smith", "1981-07-25 00:00:01", 31),
        };

        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfItemsHasChanged(ref updateDefinitions, "dummies", null, newDummies);

        // Assert
        Assert.Single(updateDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add multiple up definitions if new value is null")]
    public void SuccessfulToAddMultipleUpDefinitionsIfNewValueIsNull()
    {
        // Arrange
        List<DummyEntity> currentDummies = new List<DummyEntity>
        {
            new DummyEntity(1, "John", "Smith", "1980-04-23 00:00:01", 29),
            new DummyEntity(2, "Kim", "Smith", "1981-07-25 00:00:01", 31),
        };

        List<UpdateDefinition<DummyEntity>> updateDefinitions = new List<UpdateDefinition<DummyEntity>>();

        // Act
        UpdateHelper.AddUpDefinitionIfItemsHasChanged(ref updateDefinitions, "dummies", currentDummies, null);

        // Assert
        Assert.Single(updateDefinitions);
    }
}