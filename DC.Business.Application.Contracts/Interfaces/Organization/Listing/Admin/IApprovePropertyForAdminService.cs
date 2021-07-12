using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin
{
    public interface IApprovePropertyForAdminService : IApplicationService<int, VoidOutputDto>
    {
    }
}
