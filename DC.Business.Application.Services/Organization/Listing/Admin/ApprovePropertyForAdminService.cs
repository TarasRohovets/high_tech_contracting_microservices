using DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;
using DC.Business.Application.Services.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using System.Threading;
using DC.Business.Domain.Repositories.Organization;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;

namespace DC.Business.Application.Services.Organization.Listing.Admin
{
    public class ApprovePropertyForAdminService : BusinessService<int, VoidOutputDto>, IApprovePropertyForAdminService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;
        public ApprovePropertyForAdminService(IListingRepository listingRepository,
            IPropertiesElasticRepository propertiesElasticRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
        }
        protected override async Task<OperationResultDto<VoidOutputDto>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _listingRepository.ApprovePropertyForAdminService(id);

            var elasticPropertyToUpdate = await _propertiesElasticRepository.GetPropertyByMySqlId(id);

            if (elasticPropertyToUpdate != null)
            {
                await _propertiesElasticRepository.ApprovePropertyForAdminService(elasticPropertyToUpdate._id);
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
