using System;
using Dapper;
using DC.Core.Contracts.Infrastructure.DataAccess;
using DC.Core.DataAccess.MySql;
using DC.Core.Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace DC.Business.DataAccess.MySql.Repositories
{
    public abstract class BusinessRepository
    {
        protected const string ConnectionString = "DC.BusinessDb";

        public IDataBase BusinessDatabase { get; }

        protected BusinessRepository(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.GetConnectionString(ConnectionString)))
                throw new ConfigurationEntryNotFoundException(ConnectionString);

            BusinessDatabase = new SQLDatabase(configuration.GetConnectionString(ConnectionString));

            RegisterTypeMappings();
            RegisterSpecialMappings();
        }

        private static void RegisterTypeMappings()
        {
           // SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        private static void RegisterSpecialMappings()
        {
           // SqlMapper.SetTypeMap()
        }
    }
}
