using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Domain.ElasticEnteties;
using DC.Business.Domain.ElasticEnteties.Dto;
using DC.Business.Domain.Enums;
using DC.Business.Domain.Repositories.ElasticSearch;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;

namespace DC.Business.ElasticSearch
{
    public class PropertiesElasticRepository : IPropertiesElasticRepository
    {
        private readonly ConnectionSettings connectionSettings;
        private readonly ElasticClient elasticClient;

        public PropertiesElasticRepository(IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("configuration['elasticsearch: url']");
            if (string.IsNullOrEmpty(defaultIndex))
                throw new ArgumentNullException("configuration['elasticsearch: index']");

            connectionSettings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .EnableDebugMode()
                .ThrowExceptions(true)
                .DefaultFieldNameInferrer(b => b);

            AddDefaultMappings(connectionSettings);

            elasticClient = new ElasticClient(connectionSettings);

            CreateIndex(elasticClient);
        }

        public async Task<List<Property>> SearchPropertiesAsync(SearchCriteriaDto searchCriteria, int page = 1, int pageSize = 5)
        {
            var fieled = searchCriteria.Field;

            var searchResult = elasticClient.Search<Property>(s => s.Query(q => q
            .DisMax(dm => dm
            .Queries(
                 dq => dq.Range(c => c
                    .Boost(1.1)
                    .Field(p => p.Price)
                    .GreaterThanOrEquals(searchCriteria.PriceFrom)
                    .LessThanOrEquals(searchCriteria.PriceTo)
                    .Relation(RangeRelation.Within)
                ), 
                 dq => dq.Match(m => m
                    .Field("NumberOfBathrooms")
                    .Query(searchCriteria.Bathrooms)
                ), 
                 dq => dq.Range(c => c
                     .Boost(1.1)
                     .Field(p => p.NetAream2)
                     .GreaterThanOrEquals(searchCriteria.SizeFrom)
                     .LessThanOrEquals(searchCriteria.SizeTo)
                     .Relation(RangeRelation.Within)
                 ), 
                 dq => dq.Range(c => c
                     .Boost(1.1)
                     .Field(p => p.YearOfConstruction)
                     .GreaterThanOrEquals(searchCriteria.YearBuiltFrom)
                     .LessThanOrEquals(searchCriteria.YearBuiltTo)
                     .Relation(RangeRelation.Within)),
                 dq => dq.Term(m => m.PropertyTypeId, searchCriteria.PropertyTypeId),
                 dq => dq.Term(m => m.OperationTypeId, searchCriteria.OperationTypeId),
                 dq => dq.Term(m => m.State, PropertyState.Active)
                     )
                  )
             )
                );
            return searchResult.Documents.ToList();
        }

        public async Task<Property> GetPropertyByMySqlId(int mySqlId)
        {
            var searchResult = elasticClient.Search<Property>(s => s.Query(x => x.Term(x => x.MySqlId, mySqlId)));
            var hit = searchResult.Hits.SingleOrDefault();
            var _id = hit?.Id;
            var foundedDoc = searchResult.Documents.SingleOrDefault();
            foundedDoc._id = _id;
            return foundedDoc;
        }

        public async Task UpdatePropertyImagesByMySqlId(string documentId, List<PropertyImage> imagePaths)
        {
            var arrayOfImagePaths = new { Images = imagePaths };
            elasticClient.Update<Property, object>(documentId, u => u.Doc(arrayOfImagePaths).RetryOnConflict(1));
        }

        public async Task ApprovePropertyForAdminService(string documentId)
        {
            var state = new { State = PropertyState.Active };
            elasticClient.Update<Property, object>(documentId, u => u.Doc(state).RetryOnConflict(1));
        }

        public async Task BlockPropertyByAdminService(string documentId)
        {
            var state = new { State = PropertyState.Blocked };
            elasticClient.Update<Property, object>(documentId, u => u.Doc(state).RetryOnConflict(1));
        }

        public async Task DeletePropertyByMySqlId(long mySqlId)
        {
            var searchresult = elasticClient.DeleteByQuery<Property>(s => s.Query(x => x.Term(x => x.MySqlId, mySqlId)));
        }

        public async Task AddPropertyAsync(PropertyInsertDto property)
        {
            await elasticClient.IndexDocumentAsync(property);
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {

            var result = await elasticClient.SearchAsync<Property>(s => s.Index("properties").Size(10).Sort(q => q.Descending(p => p.Price))).ConfigureAwait(false);
            return (IEnumerable<Property>)result;
        }


        #region private methods

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Property>(propertyMapping => propertyMapping
                    .IndexName("properties"));
        }

        private static void CreateIndex(IElasticClient client)
        {
            client.Indices.CreateAsync("properties", creator => creator.Map<Property>(property => property.AutoMap()));
        }

        #endregion
    }
}
