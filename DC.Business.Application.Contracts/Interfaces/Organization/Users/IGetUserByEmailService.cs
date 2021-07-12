using System;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Core.Contracts.Application.Pipeline;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Users
{
    public interface IGetUserByEmailService : IApplicationService<string, UserDto>
    {
    }
}
