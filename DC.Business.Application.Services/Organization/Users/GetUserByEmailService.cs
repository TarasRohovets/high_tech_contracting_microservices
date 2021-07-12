using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;

namespace DC.Business.Application.Services.Organization.Users
{
    public class GetUserByEmailService : BusinessService<string, UserDto>, IGetUserByEmailService
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public GetUserByEmailService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override async Task<OperationResultDto<UserDto>> ExecuteAsync(string email, CancellationToken cancellationToken = default)
        {
            List<ErrorDto> executionErrors = ValidateInput(email);
            if (executionErrors.Any())
                return BuildOperationResultDto(executionErrors);

            //if (CurrentUser == default)
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER, nameof(User.Username), inputDto.ToString()));

            User foundUser = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);

            if (foundUser == null)
                return BuildOperationResultDto(new ErrorDto(ErrorCodes.EXPECTED_DATA_NOT_FOUND, nameof(User.Email), email));

            //if (!AuthorizationHelper.CanAccessUser(requestedUser: foundUser, requestingUser: CurrentUser))
            //    return BuildOperationResultDto(new ErrorDto(ErrorCodes.UNAUTHORIZED_ACTION_FOR_CALLING_USER, nameof(User.Username), inputDto.ToString()));

            // Temporary Mapping
            //UserDto resultUser = new UserDto()
            //{
            //    Id = foundUser.Id,
            //    City = foundUser?.City,
            //    Country = foundUser?.Country,
            //    Email = foundUser.Email,
            //    Address = foundUser?.Address,
            //    TaxNumber = foundUser?.TaxNumber,
            //    Name = foundUser?.Name,
            //    Surname = foundUser?.Surname,
            //    Active = foundUser?.Active,
            //    EmailConfirmed = foundUser?.EmailConfirmed
            //};
            UserDto resultUser = _mapper.Map<UserDto>(foundUser);

            return BuildOperationResultDto(resultUser);
        }

        private List<ErrorDto> ValidateInput(string email)
        {
            if (email == default)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(email)) };

            List<ErrorDto> validationErrors = new List<ErrorDto>();

            return validationErrors;
        }
    }
}
