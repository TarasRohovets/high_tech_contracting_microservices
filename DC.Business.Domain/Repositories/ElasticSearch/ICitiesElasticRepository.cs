using DC.Business.Application.Contracts.Dtos;
using DC.Business.Domain.ElasticEnteties;
using DC.Business.Domain.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Domain.Repositories.ElasticSearch
{
    public interface ICitiesElasticRepository
    {
        Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SuggestAsync(string query, CancellationToken cancellationToken);
        // Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SearchAsync(string query, CancellationToken cancellationToken);

        Task<IndexResponse> IndexDocumentAsync(CitiesSuggestions document, CancellationToken cancellationToken = default);
    }
}
