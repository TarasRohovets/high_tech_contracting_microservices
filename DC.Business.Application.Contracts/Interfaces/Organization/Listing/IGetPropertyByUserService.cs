using System;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Core.Contracts.Application.Pipeline;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Listing
{
    public interface IGetPropertyByUserService : IApplicationService<PropertyByIdUserIdDto, SellHouseDto>
    {
    }
}
