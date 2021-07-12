using DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Application.Services.Organization.Listing.Admin
{
    public class BlockPropertyByAdminService : BusinessService<int, VoidOutputDto>, IBlockPropertyByAdminService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;
        public BlockPropertyByAdminService(IListingRepository listingRepository, IPropertiesElasticRepository propertiesElasticRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
        }
        protected override async Task<OperationResultDto<VoidOutputDto>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _listingRepository.BlockPropertyByAdminService(id);

            var elasticPropertyToUpdate = await _propertiesElasticRepository.GetPropertyByMySqlId(id);

            if (elasticPropertyToUpdate != null)
            {
                await _propertiesElasticRepository.BlockPropertyByAdminService(elasticPropertyToUpdate._id);
            }
            else
            {
                List<ErrorDto> validationsErros = new List<ErrorDto>();
                validationsErros.Add(new ErrorDto(ErrorCodes.ELASTIC_PROPERTY_NOT_FOUND));
                return BuildOperationResultDto(validationsErros);
            }

            return BuildOperationResultDto(new VoidOutputDto());
        }
    }
}
