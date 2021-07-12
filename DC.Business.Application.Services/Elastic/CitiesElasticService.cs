using AutoMapper;
using DC.Business.Application.Contracts.Dtos;
using DC.Business.Application.Contracts.Interfaces;
using DC.Business.Domain.ElasticEnteties;
using DC.Business.Domain.Repositories.ElasticSearch;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Application.Services.Elastic
{
    public class CitiesElasticService : ICitiesElasticService
    {
        private readonly ICitiesElasticRepository _citiesElasticRepository;
        private IMapper _mapper;

        public CitiesElasticService(ICitiesElasticRepository citiesElasticRepository, IMapper mapper)
        {
            _citiesElasticRepository = citiesElasticRepository ?? throw new ArgumentNullException(nameof(citiesElasticRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //public async Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SearchAsync(string query, CancellationToken cancellationToken)
        //{
        //    var result = await _citiesElasticRepository.SearchAsync(query, cancellationToken);
        //    return result;
        //}

        public async Task<ISearchResponse<ElasticCitiesSuggestionsDto>> SuggestAsync(string query, CancellationToken cancellationToken)
        {
            var result = await _citiesElasticRepository.SuggestAsync(query, cancellationToken);

            return result;
        }
    }
}
