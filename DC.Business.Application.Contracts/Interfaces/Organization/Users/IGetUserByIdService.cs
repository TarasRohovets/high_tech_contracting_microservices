using System;
using DC.Core.Contracts.Application.Pipeline;
using DC.Business.Application.Contracts.Dtos.Organization.Users;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Users
{
    public interface IGetUserByIdService : IApplicationService<long, UserDto>
    {
    }
}
