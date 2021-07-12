using AutoMapper;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Application.Services.Organization.Listing
{
    public class SearchPropertiesService : BusinessService<SearchCriteriaDto, List<PropertyBasicDto>>, ISearchPropertiesService
    {
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;
        private IMapper _mapper;
        public SearchPropertiesService(IPropertiesElasticRepository propertiesElasticRepository, IMapper mapper)
        {
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
  
        protected override async Task<OperationResultDto<List<PropertyBasicDto>>> ExecuteAsync(SearchCriteriaDto searchCriteriaDto, CancellationToken cancellationToken = default)
        {
            ValidateInput(searchCriteriaDto);

            var result = await _propertiesElasticRepository.SearchPropertiesAsync(searchCriteriaDto);
            List<PropertyBasicDto> list = new List<PropertyBasicDto>();
            foreach(var property in result)
            {
                var mappedProperty = _mapper.Map<PropertyBasicDto>(property);

                // TODO check if no photos then skip
                if(property.Images?.Count > 0)
                {
                    mappedProperty.MainPhotoUrl = property.Images[0].ImageUrl;
                }
                
                list.Add(mappedProperty);
            }
            return BuildOperationResultDto(list);
        }

        private List<ErrorDto> ValidateInput(SearchCriteriaDto searchCriteriaDto)
        {
            List<ErrorDto> validationsErros = new List<ErrorDto>();
            if (searchCriteriaDto == null)
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(searchCriteriaDto)));
            if (string.IsNullOrEmpty(searchCriteriaDto.Field))
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(searchCriteriaDto.Field)));
            if (string.IsNullOrEmpty(searchCriteriaDto.Criteria))
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(searchCriteriaDto.Criteria)));

            return validationsErros;
        }
    }
}
