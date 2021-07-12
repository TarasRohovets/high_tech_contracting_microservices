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

namespace DC.Business.Application.Services.Organization.Listing
{
    public class GetPropertiesByUserBasicService : BusinessService<long, IEnumerable<PropertyBasicDto>>, IGetPropertiesByUserBasicService
    {
        private readonly IListingRepository _listingRepository;

        public GetPropertiesByUserBasicService(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
        }

        protected override async Task<OperationResultDto<IEnumerable<PropertyBasicDto>>> ExecuteAsync(long userId, CancellationToken cancellationToken = default)
        {
            var result = await _listingRepository.GetPropertiesByUserBasicService(userId);
            var mappedResult = result.Select(x => new PropertyBasicDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                Price = x.Price,
                NetAream2 = x.NetAream2,
                Typology = x.Typology,
                NumberOfBathrooms = x.NumberOfBathrooms,
                Country = x.Country,
                City = x.City
            });
            return BuildOperationResultDto(mappedResult);
        }
    }
}
