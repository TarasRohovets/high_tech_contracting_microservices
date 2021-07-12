using System;
using System.Collections.Generic;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Core.Contracts.Application.Pipeline;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Listing
{
    public interface IGetPropertyTypesService : IApplicationService<string, IEnumerable<PropertyTypeDto>>
    {
    }
}
