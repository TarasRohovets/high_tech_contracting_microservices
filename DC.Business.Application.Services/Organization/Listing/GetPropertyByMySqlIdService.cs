using AutoMapper;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Application.Services.Organization.Listing
{
    public class GetPropertyByMySqlIdService : BusinessService<int, PropertySellDto>, IGetPropertyByMySqlIdService
    {
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;
        private IMapper _mapper;

        public GetPropertyByMySqlIdService(IPropertiesElasticRepository propertiesElasticRepository, IMapper mapper)
        {
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override async Task<OperationResultDto<PropertySellDto>> ExecuteAsync(int inputDto, CancellationToken cancellationToken = default)
        {
            var result = await _propertiesElasticRepository.GetPropertyByMySqlId(inputDto);
            var mappedResult = _mapper.Map<PropertySellDto>(result);
            return BuildOperationResultDto(mappedResult);
        }
    }
}
