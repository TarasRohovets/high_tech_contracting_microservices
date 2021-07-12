using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;

namespace DC.Business.Application.Services.Organization.Users
{
    public class GetUserByIdService : BusinessService<long, UserDto>, IGetUserByIdService
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        protected override async Task<OperationResultDto<UserDto>> ExecuteAsync(long userId, CancellationToken cancellationToken = default)
        {
            List<ErrorDto> executionErrors = ValidateInput(userId);
            if (executionErrors.Any())
                return BuildOperationResultDto(executionErrors);

            //if (CurrentUser == default)
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER, nameof(User.Username), inputDto.ToString()));

            User foundUser = await _userRepository.GetUserByIdAsync(userId).ConfigureAwait(false);

            if (foundUser == null)
                return BuildOperationResultDto(new ErrorDto(ErrorCodes.EXPECTED_DATA_NOT_FOUND, nameof(User.Id), userId.ToString()));


            //if (!AuthorizationHelper.CanAccessUser(requestedUser: foundUser, requestingUser: CurrentUser))
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER, nameof(User.Username), inputDto.ToString()));

            // Temporary Mapping
            UserDto resultUser = new UserDto()
            {
                Id = foundUser.Id,
                City = foundUser.City,
                Country = foundUser.Country,
                Email = foundUser.Email,
                Address = foundUser.Address,
                TaxNumber = foundUser.TaxNumber,
                Name = foundUser.Name,
                Surname = foundUser.Surname
            };

            return BuildOperationResultDto(resultUser);
        }

        private List<ErrorDto> ValidateInput(long userId)
        {
            if (userId == default)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(userId)) };

            List<ErrorDto> validationErrors = new List<ErrorDto>();

            return validationErrors;
        }
    }
}
