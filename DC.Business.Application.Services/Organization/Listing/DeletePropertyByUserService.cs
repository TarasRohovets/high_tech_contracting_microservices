using System;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Services.Organization.Listing
{
    public class DeletePropertyByUserService : BusinessService<PropertyByIdUserIdDto, VoidOutputDto>, IDeletePropertyByUserService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;

        public DeletePropertyByUserService(IListingRepository listingRepository, IPropertiesElasticRepository propertiesElasticRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
        }

        protected override async Task<OperationResultDto<VoidOutputDto>> ExecuteAsync(PropertyByIdUserIdDto inputDto, CancellationToken cancellationToken = default)
        {
            await _listingRepository.DeletePorpertyByUser(inputDto);
            await _propertiesElasticRepository.DeletePropertyByMySqlId(inputDto.id);

            return BuildOperationResultDto(new VoidOutputDto());
        }
    }
}
