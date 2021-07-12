using System;
using DC.Business.Application.Contracts.Interfaces;
using DC.Business.Application.Contracts.Interfaces.Account;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin;
using DC.Business.Application.Contracts.Interfaces.Organization.Users;
using DC.Business.Application.Contracts.Interfaces.Services;
using DC.Business.Application.Services.Account;
using DC.Business.Application.Services.Elastic;
using DC.Business.Application.Services.Organization.Listing;
using DC.Business.Application.Services.Organization.Listing.Admin;
using DC.Business.Application.Services.Organization.Users;
using DC.Business.Application.Services.Services;
using DC.Business.DataAccess.MySql.Repositories.Organization;
using DC.Business.Domain.Repositories.ElasticSearch;
using DC.Business.Domain.Repositories.Organization;
using DC.Business.ElasticSearch;
using DC.Business.WebApi.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DC.Business.Bootstrap
{
    public static class BusinessBootstrapper
    {
        public static IServiceCollection RegisterBusinessApplicationServices(this IServiceCollection servicesContainer)
        {
            // Account
            servicesContainer.AddTransient<IGetTokenByEmailAndPasswordService, GetTokenByEmailAndPasswordService>();
            servicesContainer.AddTransient<ICreateUserService, CreateUserService>();
            servicesContainer.AddTransient<IUpdateUserService, UpdateUserService>();
            servicesContainer.AddTransient<IGetUserByIdService, GetUserByIdService>();
            servicesContainer.AddTransient<IGetUserByEmailService, GetUserByEmailService>();
            servicesContainer.AddTransient<ISearchUserService, SearchUserService>();
            servicesContainer.AddTransient<IListSellHouseService, ListSellHouseService>();
            servicesContainer.AddTransient<IGetOperationTypesService, GetOperationTypesService>();
            servicesContainer.AddTransient<IGetPropertyTypesService, GetPropertyTypesService>();
            servicesContainer.AddTransient<IGetPropertiesByUserBasicService, GetPropertiesByUserBasicService>();
            servicesContainer.AddTransient<IGetPropertyByUserService, GetPropertyByUserService>();
            servicesContainer.AddTransient<IDeletePropertyByUserService, DeletePropertyByUserService>(); 
            servicesContainer.AddTransient<IDeletePropertyByUserService, DeletePropertyByUserService>();
            servicesContainer.AddTransient<ISearchPropertiesService, SearchPropertiesService>(); 
            servicesContainer.AddTransient<IGetPropertyByMySqlIdService, GetPropertyByMySqlIdService>();
            //servicesContainer.AddTransient<IRecoverPasswordService, RecoverPasswordService>();
            servicesContainer.AddTransient<ISearchPropertiestForAdminService, SearchPropertiestForAdminService>();
            servicesContainer.AddTransient<IApprovePropertyForAdminService, ApprovePropertyForAdminService>();
            servicesContainer.AddTransient<IBlockPropertyByAdminService, BlockPropertyByAdminService>();


            return servicesContainer;

        }

        public static IServiceCollection RegisterBusinessRepositories(this IServiceCollection servicesContainer)
        {
            servicesContainer.AddTransient<IUserRepository, UserRepository>();
            servicesContainer.AddTransient<IListingRepository, ListingRepository>();
            servicesContainer.AddTransient<IImageRepository, ImageRepository>();
            servicesContainer.AddSingleton<IUsersElasticRepository, UsersElasticRepository>();
            servicesContainer.AddSingleton<IPropertiesElasticRepository, PropertiesElasticRepository>();
            servicesContainer.AddSingleton<ICitiesElasticRepository, CitiesElasticRepository>();
            
            return servicesContainer;
        }

        public static IServiceCollection RegisterBusinessServices(this IServiceCollection servicesContainer)
        {
            servicesContainer.AddTransient<IImageService, ImageService>();
            servicesContainer.AddTransient<ICitiesElasticService, CitiesElasticService>();
            servicesContainer.AddTransient<IIndexService, IndexService>(); 

            return servicesContainer;
        }



        //public static IServiceCollection RegisterStronglyTypedObjects(this IServiceCollection servicesContainer, IConfiguration configuration)
        //{
        //    // configure strongly typed settings objects
        //    var appSettingsSection = configuration.GetSection("AppSettings");
        //    servicesContainer.Configure<AppSettings>(appSettingsSection);

        //    return servicesContainer;
        //}


    }
}
