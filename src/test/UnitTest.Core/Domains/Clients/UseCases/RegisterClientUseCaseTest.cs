using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Clients.Commands;
using Core.Domains.Clients.Gateways;
using Core.Domains.Clients.Interfaces;
using Core.Domains.Clients.Models;
using Core.Domains.Clients.UseCases;
using Moq;
using UnitTest.Core.Commons;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.Clients.UseCases;

[ExcludeFromCodeCoverage]
public sealed class RegisterClientUseCaseTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<IRegisterClientGateway> _registerClientGateway;
    private readonly Mock<IGetDateTime> _getDateTime;
    private readonly IRegisterClientUseCase _useCase;

    public RegisterClientUseCaseTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _registerClientGateway = new Mock<IRegisterClientGateway>(MockBehavior.Strict);
        _getDateTime = new Mock<IGetDateTime>(MockBehavior.Strict);

        ServiceResolverMocker serviceResolverMocker = new ServiceResolverMocker()
            .AddService(_registerClientGateway.Object)
            .AddService(_getDateTime.Object);

        _useCase = new RegisterClientUseCase(serviceResolverMocker.Object);
    }

    private bool _disposed;

    ~RegisterClientUseCaseTest()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        _registerClientGateway.VerifyAll();
        _getDateTime.VerifyAll();
        Dispose(true);
        _outputHelper.WriteLine($"{nameof(RegisterClientUseCaseTest)} disposed.");
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Success to register client")]
    public async Task SuccessToRegisterClient()
    {
        // Arrange
        _getDateTime
            .Setup(getDateTime => getDateTime.Now())
            .Returns(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local));

        _registerClientGateway
            .Setup(gateway => gateway.ExecuteAsync(new Client
            {
                Uid = "9419357e-123b-494a-8bc3-fd17373c218b",
                Name = "White Duck Home",
                Description = "White Duck Home is a platform that brings together properties in one place.",
                CreatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                UpdatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local)
            }))
            .ReturnsAsync(new Client
            {
                Id = 1001,
                Uid = "9419357e-123b-494a-8bc3-fd17373c218b",
                Name = "White Duck Home",
                Description = "White Duck Home is a platform that brings together properties in one place.",
                CreatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local),
                UpdatedAt = new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local)
            });

        // Act
        Client client = await _useCase.ExecuteAsync(new RegisterClientCommand
        (
            uid: "9419357e-123b-494a-8bc3-fd17373c218b",
            name: "White Duck Home",
            description: "White Duck Home is a platform that brings together properties in one place."
        ));

        // Assert
        Assert.NotNull(client);

        Assert.Equal(1001, client.Id);
        Assert.Equal("9419357e-123b-494a-8bc3-fd17373c218b", client.Uid);
        Assert.Equal("White Duck Home", client.Name);
        Assert.Equal("White Duck Home is a platform that brings together properties in one place.",
            client.Description);
        Assert.Equal(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local), client.CreatedAt);
        Assert.Equal(new DateTime(2023, 10, 25, 02, 30, 01, DateTimeKind.Local), client.UpdatedAt);
    }
}