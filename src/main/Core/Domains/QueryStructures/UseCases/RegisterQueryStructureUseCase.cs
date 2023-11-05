using System.Globalization;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStructures.Commands;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Interfaces;
using Core.Domains.QueryStructures.Models;

namespace Core.Domains.QueryStructures.UseCases;

public sealed class RegisterQueryStructureUseCase : IRegisterQueryStructureUseCase
{
    private readonly IRegisterQueryStructureGateway _registerQueryStructureGateway;
    private readonly IGetDateTime _getDateTime;

    public RegisterQueryStructureUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _registerQueryStructureGateway = serviceResolver.Resolve<IRegisterQueryStructureGateway>();
        _getDateTime = serviceResolver.Resolve<IGetDateTime>();
    }

    public async Task<QueryStructure> ExecuteAsync(RegisterQueryStructureCommand command)
    {
        QueryStructure queryStructure = new QueryStructure
        {
            ClientUid = command.ClientUid,
            Name = command.Name,
            Description = command.Description,
            AiSettings = new AiSettings
            {
                UsagePercentage = byte.Parse(command.AiSettings.UsagePercentage),
                ApiKey = command.AiSettings.ApiKey,
                Model = command.AiSettings.Model,
                Temperature = float.Parse(command.AiSettings.Temperature, new CultureInfo("en-US")),
                MaxTokens = int.Parse(command.AiSettings.MaxTokens),
                PromptTemplate = command.AiSettings.PromptTemplate
            },
            CreatedAt = _getDateTime.Now(),
            UpdatedAt = _getDateTime.Now()
        };

        return await _registerQueryStructureGateway.ExecuteAsync(queryStructure);
    }
}