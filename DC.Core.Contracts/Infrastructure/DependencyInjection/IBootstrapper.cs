using System;
namespace DC.Core.Contracts.Infrastructure.DependencyInjection
{
    public interface IBootstrapper<in TServicesContainer>
    {
        void Bootstrap(TServicesContainer servicesContainer);
    }

    public interface IBootstrapper<in TServicesContainer, in TConfiguration>
    {
        void Bootstrap(TServicesContainer servicesContainer, TConfiguration configuration);
    }
}
