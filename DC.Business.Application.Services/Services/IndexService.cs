using DC.Business.Application.Contracts.Interfaces;
using DC.Business.Application.Contracts.Interfaces.Services;
using DC.Business.Domain.Entities;
using DC.Business.Domain.Repositories.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Business.Application.Services.Services
{
    

    public class IndexService : IIndexService
    {
        private readonly ICitiesElasticRepository _citiesElasticRepository;

        public IndexService(ICitiesElasticRepository citiesElasticRepository)
        {
            _citiesElasticRepository = citiesElasticRepository ?? throw new ArgumentNullException(nameof(citiesElasticRepository));
        }

        public async Task IndexCities(string city1, string city2)
        {
            var city = new CitiesSuggestions() { Suggestions = new string[] { city1, city2 }, IndexedAt = DateTime.UtcNow, UploadedAt = DateTime.UtcNow };
            await _citiesElasticRepository.IndexDocumentAsync(city);
        }
       
    }
}
