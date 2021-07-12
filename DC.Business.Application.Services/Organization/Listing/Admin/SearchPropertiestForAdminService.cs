using AutoMapper;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Dtos.Organization.Listing.Admin;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Application.Services.Organization.Listing.Admin
{
    public class SearchPropertiestForAdminService : BusinessService<SearchPaginationRequestDto<SearchPropertyForAdminRequestDto>, PagedPropertyBasicDto>, ISearchPropertiestForAdminService
    {
        private readonly IListingRepository _listingRepository;
        private IMapper _mapper;
        public SearchPropertiestForAdminService(IListingRepository listingRepository, IMapper mapper)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override async Task<OperationResultDto<PagedPropertyBasicDto>> ExecuteAsync(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto, CancellationToken cancellationToken = default)
        {
            List<ErrorDto> executionErrors = ValidateInput(inputDto);

            if (executionErrors.Any()) return BuildOperationResultDto(executionErrors);

            var result = await _listingRepository.SearchPropertiesForAdmin(inputDto);

            int totalCount = await _listingRepository.CountPropertiesForAdmin(inputDto);

            List<PropertyBasicDto> list = new List<PropertyBasicDto>();
            foreach(var prop in result?.ToList())
            {
                var mappedResult = _mapper.Map<PropertyBasicDto>(prop);
                list.Add(mappedResult);
            }

            

            var response = new PagedPropertyBasicDto() { Properties = list, TotalItems = totalCount };

            return BuildOperationResultDto(response);
        }

        private List<ErrorDto> ValidateInput(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto)
        {
            if (inputDto is null)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto>)) };

            if (inputDto.RestrictionCriteria is null)
                return new List<ErrorDto> { new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(SearchPropertyForAdminRequestDto)) };

            List<ErrorDto> validationErrors = new List<ErrorDto>();
            if (inputDto.RowsPerPage < 1)
                validationErrors.Add(new ErrorDto(ErrorCodes.VALUE_OUT_OF_RANGE, nameof(inputDto.RowsPerPage)));


            if (inputDto.PageNumber < 1)
                validationErrors.Add(new ErrorDto(ErrorCodes.VALUE_OUT_OF_RANGE, nameof(inputDto.PageNumber)));


            return validationErrors;
        }
    }
}
