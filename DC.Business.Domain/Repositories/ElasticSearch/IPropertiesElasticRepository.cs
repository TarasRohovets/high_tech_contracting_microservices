using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Domain.ElasticEnteties;
using DC.Business.Domain.ElasticEnteties.Dto;

namespace DC.Business.Domain.Repositories.ElasticSearch
{
    public interface IPropertiesElasticRepository
    {
        Task<List<Property>> SearchPropertiesAsync(SearchCriteriaDto searchCriteria, int page = 1, int pageSize = 5);
        Task<Property> GetPropertyByMySqlId(int mySqlId);
        Task AddPropertyAsync(PropertyInsertDto property);
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task DeletePropertyByMySqlId(long mySqlId);

        Task UpdatePropertyImagesByMySqlId(string documentId, List<PropertyImage> imagePaths);
        Task ApprovePropertyForAdminService(string documentId);
        Task BlockPropertyByAdminService(string documentId);
    }
}
