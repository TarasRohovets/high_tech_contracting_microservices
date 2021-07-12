using System;
using DC.Business.Bootstrap;
using DC.Core.Bootstrap;
using DC.Core.Contracts.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DC.Business.WebApi
{
    internal sealed class DependencyInjection : IBootstrapper<IServiceCollection, IConfiguration>
    {
        public void Bootstrap(IServiceCollection servicesContainer, IConfiguration configuration)
        {
            // Register Services Container Instance for later usage in Application Services
            servicesContainer.AddSingleton<IServiceProvider>(servicesContainer.BuildServiceProvider());
            // Register Configuration Instance for later usage in Application Services
            servicesContainer.AddSingleton(configuration);

            servicesContainer.RegisterBusinessApplicationServices()
                             .RegisterBusinessServices()
                             .RegisterBusinessRepositories()
                             .RegisterJwtTokenServices(configuration)
                             .RegisterRabbitMQPublisher(configuration);
        }
    }
}
