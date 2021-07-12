using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.Organization;
using DC.Business.Domain.Entities.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using System.Linq;
using DC.Business.Domain.ViewModel;

namespace DC.Business.Application.Services.Organization.Users
{
    public class SearchUserService : BusinessService<SearchPaginationRequestDto<UserSearchInputDto>, IEnumerable<UserDto>>, ISearchUserService
    {
        private readonly IUserRepository _userRepository;

        public SearchUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        protected override async Task<OperationResultDto<IEnumerable<UserDto>>> ExecuteAsync(SearchPaginationRequestDto<UserSearchInputDto> inputDto, CancellationToken cancellationToken = default)
        {
            List<ErrorDto> executionErrors = ValidateInput(inputDto);
            if (executionErrors.Any()) return BuildOperationResultDto(executionErrors);

            // temporaty mappings
            UserSearchInput searchInput = new UserSearchInput()
            {
                Name = inputDto.RestrictionCriteria.Name,
                Email = inputDto.RestrictionCriteria.Email,
                Username = inputDto.RestrictionCriteria.Username

            };

            // temporaty mappings
            SearchPaginationRequestDto<UserSearchInput> spDto = new SearchPaginationRequestDto<UserSearchInput>()
            {
                RestrictionCriteria = searchInput,
                OrderBy = inputDto.OrderBy,
                OrderDescending = inputDto.OrderDescending,
                PageNumber = inputDto.PageNumber,
                RowsPerPage = inputDto.RowsPerPage
            };

            IEnumerable<User> searchResult = await _userRepository.SearchUsers(spDto).ConfigureAwait(false);
            List<UserDto> searchResultMapped = new List<UserDto>();

            foreach (var foundUser in searchResult) { 
            // Temporary Mapping
            UserDto resultUser = new UserDto()
            {
                Id = foundUser.Id,
                City = foundUser?.City,
                Country = foundUser?.Country,
                Email = foundUser.Email,
                Address = foundUser?.Address,
                TaxNumber = foundUser?.TaxNumber,
                Name = foundUser?.Name,
                Surname = foundUser?.Surname
            };
                searchResultMapped.Add(resultUser);
            }

            return BuildOperationResultDto(searchResultMapped);
        }


        private List<ErrorDto> ValidateInput(SearchPaginationRequestDto<UserSearchInputDto> inputDto)
        {
            if (inputDto is null)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(SearchPaginationRequestDto<UserSearchInputDto>)) };

            if (inputDto.RestrictionCriteria is null)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(UserSearchInputDto)) };

            List<ErrorDto> validationErrors = new List<ErrorDto>();
            if (inputDto.RowsPerPage < 1)
                validationErrors.Add(new ErrorDto(ErrorCodes.VALUE_OUT_OF_RANGE, nameof(inputDto.RowsPerPage)));


            if (inputDto.PageNumber < 1)
                validationErrors.Add(new ErrorDto(ErrorCodes.VALUE_OUT_OF_RANGE, nameof(inputDto.PageNumber)));


            return validationErrors;
        }
    }
}
