using System;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;

namespace DC.Business.Application.Services.Organization.Listing
{
    public class GetPropertyByUserService : BusinessService<PropertyByIdUserIdDto, SellHouseDto>, IGetPropertyByUserService
    {
        private readonly IListingRepository _listingRepository;

        public GetPropertyByUserService(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
        }

        protected override async Task<OperationResultDto<SellHouseDto>> ExecuteAsync(PropertyByIdUserIdDto inputDto, CancellationToken cancellationToken = default)
        {
            var result = await _listingRepository.GetPropertyByUser(inputDto);
            var property = new SellHouseDto()
            {
                UserId = result.UserId,
                PropertyTypeId = result.PropertyTypeId,
                OperationTypeId = result.OperationTypeId,
                Price = result.Price,
                NetAream2 = result.NetAream2,
                PriceNetAream2 = result.PriceNetAream2,
                GrossAream2 = result.GrossAream2,
                Typology = result.Typology,
                Floor = result.Floor,
                YearOfConstruction = result.YearOfConstruction,
                NumberOfBathrooms = result.NumberOfBathrooms,
                EnerergyCertificate = result.EnerergyCertificate,
                Country = result.Country,
                City = result.City,
                Address = result.Address,
                Description = result.Description,
                Latitude = result.Latitude,
                Longitude = result.Longitude
            };
            return BuildOperationResultDto(property);
        }
    }
}
