#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Extensions;

public static class MongoCollectionQueryByPageExtensions
{
    public static async Task<(int totalPages, long totalOfElements, IReadOnlyList<TDocument> content)> AggregateByPage<TDocument>(
        this IMongoCollection<TDocument> collection,
        FilterDefinition<TDocument> filterDefinition,
        SortDefinition<TDocument> sortDefinition,
        int pageNumber,
        int pageSize)
    {
        AggregateFacet<TDocument, AggregateCountResult> countFacet = AggregateFacet.Create("count",
            PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<TDocument>()
            }));

        AggregateFacet<TDocument, TDocument> dataFacet = AggregateFacet.Create("content",
            PipelineDefinition<TDocument, TDocument>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Sort(sortDefinition),
                PipelineStageDefinitionBuilder.Skip<TDocument>(pageNumber * pageSize),
                PipelineStageDefinitionBuilder.Limit<TDocument>(pageSize)
            }));

        List<AggregateFacetResults> aggregation = await collection.Aggregate()
            .Match(filterDefinition)
            .Facet(countFacet, dataFacet)
            .ToListAsync();

        long totalOfElements = GetTotalOfElements(aggregation);

        int totalPages = (int) Math.Ceiling((float) totalOfElements / pageSize);

        IReadOnlyList<TDocument> content = aggregation.FirstOrDefault()!
            .Facets.FirstOrDefault(element => element.Name == "content")!
            .Output<TDocument>();

        return (totalPages, totalOfElements, content);
    }

    private static long GetTotalOfElements(IReadOnlyList<AggregateFacetResults> aggregation)
    {
        AggregateFacetResult? aggregateFacetResult = aggregation[0].Facets.FirstOrDefault(element => element.Name == "count");
        return aggregateFacetResult is not null && aggregateFacetResult.Output<AggregateCountResult>().Count != 0
            ? aggregateFacetResult.Output<AggregateCountResult>()[0].Count
            : 0;
    }
}