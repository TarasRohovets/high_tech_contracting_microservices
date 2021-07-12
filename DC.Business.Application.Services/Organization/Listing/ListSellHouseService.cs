using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Services.Pipeline;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Enums;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos;
using DC.Core.Contracts.Application.Pipeline.Dtos.Errors;
using DC.Core.Contracts.Application.Pipeline.Dtos.Output;

namespace DC.Business.Application.Services.Organization.Listing
{
    public class ListSellHouseService : BusinessService<SellHouseDto, ulong>, IListSellHouseService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IPropertiesElasticRepository _propertiesElasticRepository;

        public ListSellHouseService(IListingRepository listingRepository, IPropertiesElasticRepository propertiesElasticRepository)
        {
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _propertiesElasticRepository = propertiesElasticRepository ?? throw new ArgumentNullException(nameof(propertiesElasticRepository));
        }

        protected override async Task<OperationResultDto<ulong>> ExecuteAsync(SellHouseDto sellHouseDto, CancellationToken cancellationToken = default)
        {
            List<ErrorDto> executionErrors = ValidateInput(sellHouseDto);
            if (executionErrors.Any()) return BuildOperationResultDto(executionErrors);

            // TODO use mapper

            var property = new Property()
            {
                UserId = sellHouseDto.UserId, 
                PropertyTypeId = sellHouseDto.PropertyTypeId, 
                OperationTypeId = sellHouseDto.OperationTypeId, 
                Price = sellHouseDto.Price,
                NetAream2 = sellHouseDto.NetAream2,
                PriceNetAream2 = sellHouseDto.PriceNetAream2,
                GrossAream2 = sellHouseDto.GrossAream2,
                Typology = sellHouseDto.Typology,
                Floor = sellHouseDto.Floor,
                YearOfConstruction = sellHouseDto.YearOfConstruction,
                NumberOfBathrooms = sellHouseDto.NumberOfBathrooms,
                EnerergyCertificate = sellHouseDto.EnerergyCertificate,
                Country = sellHouseDto.Country,
                City = sellHouseDto.City,
                Address = sellHouseDto.Address,
                Description = sellHouseDto.Description,
                State = PropertyState.NotApproved,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Deleted = false,
                Latitude = sellHouseDto.Latitude,
                Longitude = sellHouseDto.Longitude
            };

            // TODO get DB id
            var mySqlId = await _listingRepository.ListProperty(property);

            // TODO mapper
            // TODO add id to the entity
            var elasticProperty = new Domain.ElasticEnteties.Dto.PropertyInsertDto()
            {
                UserId = sellHouseDto.UserId,
                MySqlId = mySqlId,
                PropertyTypeId = sellHouseDto.PropertyTypeId,
                OperationTypeId = sellHouseDto.OperationTypeId,
                Price = sellHouseDto.Price,
                NetAream2 = sellHouseDto.NetAream2,
                PriceNetAream2 = sellHouseDto.PriceNetAream2,
                GrossAream2 = sellHouseDto.GrossAream2,
                Typology = sellHouseDto.Typology,
                Floor = sellHouseDto.Floor,
                YearOfConstruction = sellHouseDto.YearOfConstruction,
                NumberOfBathrooms = sellHouseDto.NumberOfBathrooms,
                EnerergyCertificate = sellHouseDto.EnerergyCertificate,
                Country = sellHouseDto.Country,
                City = sellHouseDto.City,
                Address = sellHouseDto.Address,
                Description = sellHouseDto.Description,
                Latitude = sellHouseDto.Latitude,
                Longitude = sellHouseDto.Longitude,
                State = PropertyState.NotApproved
            };

            await _propertiesElasticRepository.AddPropertyAsync(elasticProperty);

            return BuildOperationResultDto(mySqlId);
        }

        private List<ErrorDto> ValidateInput(SellHouseDto sellHouseDt)
        {
            List<ErrorDto> validationsErros = new List<ErrorDto>();
            if (sellHouseDt == null)
                validationsErros.Add(new ErrorDto(ErrorCodes.REQUIRED_FILED_IS_EMPTY, nameof(sellHouseDt)));

            return validationsErros;
        }
    }
}
