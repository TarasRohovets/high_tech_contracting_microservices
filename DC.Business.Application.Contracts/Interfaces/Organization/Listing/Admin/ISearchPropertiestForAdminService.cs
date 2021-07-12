using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Dtos.Organization.Listing.Admin;
using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin
{
    public interface ISearchPropertiestForAdminService : IApplicationService<SearchPaginationRequestDto<SearchPropertyForAdminRequestDto>, PagedPropertyBasicDto>
    {
    }
}
