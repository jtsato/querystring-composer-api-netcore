using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Commands;
using Core.Domains.QueryStructures.Interfaces;
using Core.Domains.QueryStructures.Models;
using Core.Domains.QueryStructures.UseCases;
using Core.Exceptions;
using Moq;
using UnitTest.Core.Commons;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStructures.UseCases;

public sealed class AddQueryStructureItemUseCaseTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<IGetQueryStructureByIdGateway> _getQueryStructureByIdGateway;
    private readonly Mock<IUpdateQueryStructureGateway> _updateQueryStructureGateway;
    private readonly Mock<IGetDateTime> _getDateTime;
    private readonly IAddQueryStructureItemUseCase _useCase;

    public AddQueryStructureItemUseCaseTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _getQueryStructureByIdGateway = new Mock<IGetQueryStructureByIdGateway>(MockBehavior.Strict);
        _updateQueryStructureGateway = new Mock<IUpdateQueryStructureGateway>(MockBehavior.Strict);
        _getDateTime = new Mock<IGetDateTime>(MockBehavior.Strict);

        ServiceResolverMocker serviceResolverMocker = new ServiceResolverMocker()
            .AddService(_getQueryStructureByIdGateway.Object)
            .AddService(_updateQueryStructureGateway.Object)
            .AddService(_getDateTime.Object);

        _useCase = new AddQueryStructureItemUseCase(serviceResolverMocker.Object);
    }

    private bool _disposed;

    ~AddQueryStructureItemUseCaseTest() => Dispose(false);

    public void Dispose()
    {
        _getQueryStructureByIdGateway.VerifyAll();
        _updateQueryStructureGateway.VerifyAll();
        _getDateTime.VerifyAll();
        Dispose(true);
        _outputHelper.WriteLine($"{nameof(AddQueryStructureItemUseCaseTest)} disposed.");
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }
    
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to add query structure item due to query structure not found")]
    public async Task FailToAddQueryStructureItemDueToQueryStructureNotFound()
    {
        // Arrange
        _getQueryStructureByIdGateway
            .Setup(gateway => gateway.ExecuteAsync(1))
            .ReturnsAsync(Optional<QueryStructure>.Empty);
        
        // Act
        Exception exception = await Record.ExceptionAsync(() => _useCase.ExecuteAsync(new AddQueryStructureItemCommand
        (
            queryStructureId: 1,
            name: "minBedrooms",
            description: "Minimum Bedrooms",
            isCountable: true,
            waitForConfirmationWords: false,
            confirmationWords: new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
            revocationWords: new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"}
        )));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<NotFoundException>(exception);
        Assert.Equal("ValidationQueryStructureByIdNotFound", exception.Message);
        Assert.Equal(1, ((NotFoundException) exception).Parameters[0]);
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to add query structure item")]
    public async Task SuccessToAddQueryStructureItem()
    {
        // Arrange
        _getQueryStructureByIdGateway
            .Setup(gateway => gateway.ExecuteAsync(1))
            .ReturnsAsync(Optional<QueryStructure>.Of
            (
                new QueryStructure
                {
                    ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                    Name = "White Duck Home",
                    Description = "White Duck Home is a platform that brings together properties in one place.",
                    AiSettings = new AiSettings
                    {
                        UsagePercentage = 50,
                        ApiKey = "api_key",
                        Model = "model",
                        Temperature = 0.5f,
                        MaxTokens = 1024,
                        PromptTemplate = "prompt_template"
                    },
                    CreatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                    UpdatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local)
                }
            ));
        
        _getDateTime
            .Setup(service => service.Now())
            .Returns(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local));
        
        _updateQueryStructureGateway.Setup(gateway => gateway.ExecuteAsync(It.IsAny<QueryStructure>()))
            .ReturnsAsync(new QueryStructure
            {
                Id = 1,
                ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                Name = "White Duck Home",
                Description = "White Duck Home is a platform that brings together properties in one place.",
                AiSettings = new AiSettings
                {
                    UsagePercentage = 50,
                    ApiKey = "api_key",
                    Model = "model",
                    Temperature = 0.5f,
                    MaxTokens = 1024,
                    PromptTemplate = "prompt_template"
                },
                CreatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                UpdatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                Items = new List<Item>
                {
                    new Item
                    {
                        Name = "minBedrooms",
                        Description = "Minimum Bedrooms",
                        IsCountable = true,
                        WaitForConfirmationWords = false,
                        ConfirmationWords = new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
                        RevocationWords = new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"}
                    }
                }
            });

        // Act
        QueryStructure queryStructure = await _useCase.ExecuteAsync(new AddQueryStructureItemCommand
        (
            queryStructureId: 1,
            name: "minBedrooms",
            description: "Minimum Bedrooms",
            isCountable: true,
            waitForConfirmationWords: false,
            confirmationWords: new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"},
            revocationWords: new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"}
        ));

        // Assert
        Assert.Equal(1, queryStructure.Id);
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", queryStructure.ClientUid);
        Assert.Equal("White Duck Home", queryStructure.Name);
        Assert.Equal("White Duck Home is a platform that brings together properties in one place.", queryStructure.Description);
        Assert.Equal(50, queryStructure.AiSettings.UsagePercentage);
        Assert.Equal("api_key", queryStructure.AiSettings.ApiKey);
        Assert.Equal("model", queryStructure.AiSettings.Model);
        Assert.Equal(0.5f, queryStructure.AiSettings.Temperature);
        Assert.Equal(1024, queryStructure.AiSettings.MaxTokens);
        Assert.Equal("prompt_template", queryStructure.AiSettings.PromptTemplate);
        Assert.Equal(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local), queryStructure.CreatedAt);
        Assert.Equal(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local), queryStructure.UpdatedAt);
        Assert.Single(queryStructure.Items);
        Assert.Equal("minBedrooms", queryStructure.Items[0].Name);
        Assert.Equal("Minimum Bedrooms", queryStructure.Items[0].Description);
        Assert.True(queryStructure.Items[0].IsCountable);
        Assert.False(queryStructure.Items[0].WaitForConfirmationWords);
        Assert.Equal(new List<string> {"com", "acima", "desde", "maior", "mais", "min", "mínimo", "partir"}, queryStructure.Items[0].ConfirmationWords);
        Assert.Equal(new List<string> {"abaixo", "antes", "a", "à", "á", "até", "inferior", "max", "máx", "máximo"}, queryStructure.Items[0].RevocationWords);
    }
}