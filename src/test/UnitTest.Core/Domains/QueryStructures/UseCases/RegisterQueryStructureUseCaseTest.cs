using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Commands;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Interfaces;
using Core.Domains.QueryStructures.Models;
using Core.Domains.QueryStructures.UseCases;
using Moq;
using UnitTest.Core.Commons;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.QueryStructures.UseCases;

public sealed class RegisterQueryStructureUseCaseTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<IRegisterQueryStructureGateway> _registerQueryStructureGateway;
    private readonly Mock<IGetDateTime> _getDateTime;
    private readonly IRegisterQueryStructureUseCase _useCase;

    public RegisterQueryStructureUseCaseTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _registerQueryStructureGateway = new Mock<IRegisterQueryStructureGateway>(MockBehavior.Strict);
        _getDateTime = new Mock<IGetDateTime>(MockBehavior.Strict);

        ServiceResolverMocker serviceResolverMocker = new ServiceResolverMocker()
            .AddService(_registerQueryStructureGateway.Object)
            .AddService(_getDateTime.Object);

        _useCase = new RegisterQueryStructureUseCase(serviceResolverMocker.Object);
    }

    private bool _disposed;

    ~RegisterQueryStructureUseCaseTest() => Dispose(false);

    public void Dispose()
    {
        _registerQueryStructureGateway.VerifyAll();
        _getDateTime.VerifyAll();
        Dispose(true);
        _outputHelper.WriteLine($"{nameof(RegisterQueryStructureUseCaseTest)} disposed.");
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to register query structure")]
    public async Task SuccessToRegisterQueryStructure()
    {
        // Arrange
        _registerQueryStructureGateway
            .Setup(gateway => gateway.ExecuteAsync(
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
                        Temperature = 0.5,
                        MaxTokens = 1024,
                        PromptTemplate = "prompt_template"
                    },
                    CreatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                    UpdatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local)
                }
            ))
            .ReturnsAsync(new QueryStructure
            {
                Id = 1001,
                ClientUid = "9419357e-123b-494a-8bc3-fd17373c218b",
                Name = "White Duck Home",
                Description = "White Duck Home is a platform that brings together properties in one place.",
                AiSettings = new AiSettings
                {
                    UsagePercentage = 50,
                    ApiKey = "api_key",
                    Model = "model",
                    Temperature = 0.5,
                    MaxTokens = 1024,
                    PromptTemplate = "prompt_template"
                },
                CreatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                UpdatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local)
            });

        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local));

        // Act
        QueryStructure queryStructure = await _useCase.ExecuteAsync(new RegisterQueryStructureCommand
        (
            clientUid: "9419357e-123b-494a-8bc3-fd17373c218b",
            name: "White Duck Home",
            description: "White Duck Home is a platform that brings together properties in one place.",
            aiSettings: new AiSettingsCommand
            (
                usagePercentage: "50",
                apiKey: "api_key",
                model: "model",
                temperature: "0.5",
                maxTokens: "1024",
                promptTemplate: "prompt_template"
            )
        ));

        // Assert
        Assert.NotNull(queryStructure);

        Assert.Equal(1001, queryStructure.Id);
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", queryStructure.ClientUid);
        Assert.Equal("White Duck Home", queryStructure.Name);
        Assert.Equal("White Duck Home is a platform that brings together properties in one place.", queryStructure.Description);
        Assert.Equal(50, queryStructure.AiSettings.UsagePercentage);
        Assert.Equal("api_key", queryStructure.AiSettings.ApiKey);
        Assert.Equal("model", queryStructure.AiSettings.Model);
        Assert.Equal(0.5, queryStructure.AiSettings.Temperature);
        Assert.Equal(1024, queryStructure.AiSettings.MaxTokens);
        Assert.Equal("prompt_template", queryStructure.AiSettings.PromptTemplate);
        Assert.Equal(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local), queryStructure.CreatedAt);
        Assert.Equal(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local), queryStructure.UpdatedAt);
    }
}
