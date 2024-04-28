using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.QueryStrings.Commands;
using Core.Domains.QueryStrings.Gateways;
using Core.Domains.QueryStrings.Interfaces;
using Core.Domains.QueryStrings.Models;
using Core.Domains.QueryStructures.Gateways;
using Core.Domains.QueryStructures.Models;
using Core.Exceptions;

namespace Core.Domains.QueryStrings.UseCases;

public class BuildQueryStringUseCase : IBuildQueryStringUseCase
{
    private readonly IGetQueryStructureByNameGateway _getQueryStructureByNameGateway;
    private readonly IBuildQueryStringWithAiGateway _buildQueryStringWithAiGateway;
    private readonly IGetDateTime _getDateTime;

    public BuildQueryStringUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _getQueryStructureByNameGateway = serviceResolver.Resolve<IGetQueryStructureByNameGateway>();
        _buildQueryStringWithAiGateway = serviceResolver.Resolve<IBuildQueryStringWithAiGateway>();
        _getDateTime = serviceResolver.Resolve<IGetDateTime>();
    }

    public async Task<Output> ExecuteAsync(BuildQueryStringCommand command)
    {
        QueryStructure queryStructure = await GetQueryStructure(command.ClientUid, command.QueryName);

        int percentageRange = RandomNumberGeneratorByRange.Generate(1, 100);
        
        bool shouldUseAi = queryStructure.AiSettings.UsagePercentage >= percentageRange;

        if (shouldUseAi)
        {
            Optional<string> optional = await _buildQueryStringWithAiGateway.ExecuteAsync(queryStructure, command.SearchTerms);
            if (optional.HasValue()) return BuildOutput(command, optional.GetValue(), true);
        }

        string queryString = await ManualQueryBuilderHelper.Build(queryStructure, command.SearchTerms);

        return BuildOutput(command, queryString, false);
    }

    private async Task<QueryStructure> GetQueryStructure(string clientUid, string queryName)
    {
        Optional<QueryStructure> optional = await _getQueryStructureByNameGateway.ExecuteAsync(clientUid, queryName);

        return optional.OrElseThrow(() => new NotFoundException("ValidationQueryStructureByClientAndNameNotFound", clientUid, queryName));
    }

    private Output BuildOutput(BuildQueryStringCommand command, string queryString, bool createdByAi)
    {
        return new Output
        {
            ClientUid = command.ClientUid,
            QueryName = command.QueryName,
            SearchTerms = command.SearchTerms,
            QueryString = queryString,
            CreatedAt = _getDateTime.Now(),
            CreatedByAi = createdByAi
        };
    }
}
