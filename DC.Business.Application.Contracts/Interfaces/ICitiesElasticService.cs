using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos;

namespace DC.Business.Application.Contracts.Interfaces
{
    public interface ICitiesElasticService
    {
        // Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SearchAsync(string query, CancellationToken cancellationToken);

        Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SuggestAsync(string query, CancellationToken cancellationToken);
    }
}
