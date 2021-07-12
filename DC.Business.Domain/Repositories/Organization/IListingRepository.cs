using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Dtos.Organization.Listing.Admin;
using DC.Business.Domain.Entities.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;

namespace DC.Business.Domain.Repositories.Organization
{
    public interface IListingRepository
    {
        Task<ulong> ListProperty(Property property);
        Task<IEnumerable<OperationType>> GetOperationTypes();
        Task<IEnumerable<PropertyType>> GetPropertyTypes();
        Task<IEnumerable<Property>> GetPropertiesByUserBasicService(long userId);
        Task<Property> GetPropertyByUser(PropertyByIdUserIdDto input);
        Task<Property> GetPropertyByEmail(string email);
        Task<Property> GetPropertyById(long propertyId);
        Task DeletePorpertyByUser(PropertyByIdUserIdDto input);
        Task<IEnumerable<Property>> SearchPropertiesForAdmin(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto);
        Task<int> CountPropertiesForAdmin(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto);
        Task ApprovePropertyForAdminService(int mySqlId);
        Task BlockPropertyByAdminService(int mySqlId);
    }
}
