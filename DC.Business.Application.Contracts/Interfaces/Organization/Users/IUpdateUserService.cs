using System;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Users
{
    public interface IUpdateUserService : IApplicationService<UserDto, VoidOutputDto>
    {
    }
}
