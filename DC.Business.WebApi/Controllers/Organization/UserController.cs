using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DC.Business.WebApi.Controllers.Organization
{
    [Route("api/organization/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserService _createUserService;
        private readonly IUpdateUserService _updateUserService;
        private readonly IGetUserByIdService _getUserByIdService;
        private readonly IGetUserByEmailService _getUserByEmailService;
        private readonly ISearchUserService _searchUserService;

        public UserController(ICreateUserService createUserService,
            IUpdateUserService updateUserService,
            IGetUserByIdService getUserByIdService,
            IGetUserByEmailService getUserByEmailService,
            ISearchUserService searchUserService)
        {
            _createUserService = createUserService ?? throw new ArgumentNullException(nameof(createUserService));
            _updateUserService = updateUserService ?? throw new ArgumentNullException(nameof(updateUserService));
            _getUserByIdService = getUserByIdService ?? throw new ArgumentNullException(nameof(getUserByIdService));
            _getUserByEmailService = getUserByEmailService ?? throw new ArgumentNullException(nameof(getUserByEmailService));
            _searchUserService = searchUserService ?? throw new ArgumentNullException(nameof(searchUserService));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createNewUser")]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateUserDto user, CancellationToken cancellationToken = default)
        {
            var result = await _createUserService.ExecuteServiceAsync(user, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute(Name = "userId")]long userId, CancellationToken cancellationToken = default)
        {
            OperationResultDto<UserDto> result = await _getUserByIdService.ExecuteServiceAsync(userId, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> GetUserByEmailAsync([FromRoute(Name = "email")] string email, CancellationToken cancellationToken = default)
        {
            OperationResultDto<UserDto> result = await _getUserByEmailService.ExecuteServiceAsync(email, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("setPassword")]
        public async Task<IActionResult> SetUserPassword(SetPasswordDto setPasswordDto, CancellationToken cancellationToken = default)
        {
            return Ok();
        }

        [HttpPost]
        [Route("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user, CancellationToken cancellationToken = default)
        {
            var result = await _updateUserService.ExecuteServiceAsync(user, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [AllowAnonymous] // TODO To remove
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchUsers(SearchPaginationRequestDto<UserSearchInputDto> inputDto, CancellationToken cancellationToken = default)
        {
            var result = await _searchUserService.ExecuteServiceAsync(inputDto, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
