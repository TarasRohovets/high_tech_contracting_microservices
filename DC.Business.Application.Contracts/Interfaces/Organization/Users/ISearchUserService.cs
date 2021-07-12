using System;
using System.Collections.Generic;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Core.Contracts.Application.Pipeline;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;

namespace DC.Business.Application.Contracts.Interfaces.Organization.Users
{
    public interface ISearchUserService : IApplicationService<SearchPaginationRequestDto<UserSearchInputDto>, IEnumerable<UserDto>>
    {
    }
}
