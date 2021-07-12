using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Services.Organization.Listing
{
    public class GetOperationTypesService : BusinessService<string, IEnumerable<OperationTypeDto>>, IGetOperationTypesService
    {
        private readonly IListingRepository _listingRepository;

        public GetOperationTypesService(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
        }

        protected override async Task<OperationResultDto<IEnumerable<OperationTypeDto>>> ExecuteAsync(string inputDto, CancellationToken cancellationToken = default)
        {
            var result = await _listingRepository.GetOperationTypes();
            var mappedResult = result.Select(x => new OperationTypeDto()
            {
                Id = x.Id,
                Type = x.Type
            });
            return BuildOperationResultDto(mappedResult);
        }
    }
}
