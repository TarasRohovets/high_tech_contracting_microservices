using System;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Listing
{
    public interface IListSellHouseService : IApplicationService<SellHouseDto, ulong>
    {
    }
}
