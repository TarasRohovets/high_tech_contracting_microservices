using DC.Business.Application.Contracts.Dtos;
using DC.Business.Domain.ElasticEnteties;
using DC.Business.Domain.Entities;
using DC.Business.Domain.Repositories.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.ElasticSearch
{
    public class CitiesElasticRepository : ICitiesElasticRepository
    {
        private readonly ConnectionSettings connectionSettings;
        private readonly ElasticClient elasticClient;

        public CitiesElasticRepository(IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index.cities"];

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("configuration['elasticsearch: url']");
            if (string.IsNullOrEmpty(defaultIndex))
                throw new ArgumentNullException("configuration['elasticsearch: index.cities']");

            connectionSettings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .EnableDebugMode()
                .ThrowExceptions(true)
                .DefaultFieldNameInferrer(b => b);

            // AddDefaultMappings(connectionSettings);

            elasticClient = new ElasticClient(connectionSettings);

            CreateIndex(elasticClient, new CancellationToken());
        }

        public Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SuggestAsync(string query, CancellationToken cancellationToken)
        {
            return elasticClient.SearchAsync<ElasticCitiesSuggestionsDto>(x => x
                // Query this Index:
                .Index("cities")
                // Suggest Titles:
                .Suggest(s => s
                    .Completion("suggest", x => x
                        .Prefix(query)
                        .SkipDuplicates(true)
                        .Field(x => x.Suggestions))), cancellationToken);
        }

        //public Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SearchAsync(string query, CancellationToken cancellationToken)
        //{
        //    return elasticClient.SearchAsync<ElasticCitiesSuggestionsDto>(document => document
        //        // Query this Index:
        //        .Index("cities")
        //        // Now kick off the query:
        //        .Query(q => q.MultiMatch(mm => mm
        //            .Query(query)
        //            .Type(TextQueryType.BoolPrefix)
        //            .Fields(f => f
        //                .Field(d => d.Keywords)))), cancellationToken);
        //}

        public async Task<IndexResponse> IndexDocumentAsync(CitiesSuggestions document, CancellationToken cancellationToken = default)
        {
            return await elasticClient.IndexAsync(new ElasticCitiesSuggestionsDto
            {
                //Id = document.Id.ToString(),
                //Title = document.Title,
                Suggestions = document.Suggestions,
                // Keywords = document.Suggestions,
                IndexedOn = DateTime.UtcNow
            }, null ,cancellationToken);
        }

        //public async Task<DeleteResponse> DeleteDocumentAsync(CitiesSuggestions document, CancellationToken cancellationToken)
        //{
        //    return await elasticClient.DeleteAsync();
        //}

        #region private methods

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<ElasticCitiesSuggestionsDto>(suggestions => suggestions
                    .IndexName("cities"));
        }

        private async Task CreateIndex(IElasticClient client, CancellationToken cancellationToken)
        {
            //client.Indices.CreateAsync("cities", creator => creator.Map<CitiesSuggestions>(suggestions => suggestions.AutoMap()));

            await client.Indices.CreateAsync("cities", descriptor =>
            {
                return descriptor.Map<ElasticCitiesSuggestions>(mapping => mapping
                    .Properties(properties => properties
                        .Text(textField => textField.Name(document => document.Id))
                        .Text(textField => textField.Name(document => document.Title))
                        .Date(dateField => dateField.Name(document => document.IndexedOn))
                        //.Keyword(keywordField => keywordField.Name(document => document.Keywords))
                        .Completion(completionField => completionField.Name(document => document.Suggestions))));
            }, cancellationToken);
        }

        #endregion
    }
}
