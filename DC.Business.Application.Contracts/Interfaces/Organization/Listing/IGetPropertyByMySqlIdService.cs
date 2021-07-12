using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Core.Contracts.Application.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Listing
{
    public interface IGetPropertyByMySqlIdService : IApplicationService<int, PropertySellDto>
    {
    }
}
