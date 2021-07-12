using System;
using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Users
{
    public interface IRecoverPasswordService : IApplicationService<string, VoidOutputDto>
    {
    }
}
