using AutoMapper;
using DC.Business.Application.Contracts.Dtos.Image;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Domain.Entities.Organization;

namespace DC.Business.WebApi.Infrastructure.Automapper
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<DC.Business.Domain.ElasticEnteties.Property, PropertyBasicDto>();
            CreateMap<Property, PropertyBasicDto>();
            CreateMap<DC.Business.Domain.ElasticEnteties.Property, PropertySellDto>(); 
            CreateMap<DC.Business.Domain.ElasticEnteties.PropertyImage, PropertyImageDto>(); 
        }
    }
}
