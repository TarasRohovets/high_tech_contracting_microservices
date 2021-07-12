using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Nest;

namespace DC.Business.ElasticSearch
{
    public class UsersElasticRepository : IUsersElasticRepository
    {
        private readonly ConnectionSettings connectionSettings;
        private readonly ElasticClient elasticClient;

        public UsersElasticRepository(IConfiguration configuration)
        {

            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("configuration['elasticsearch: url']");
            if (string.IsNullOrEmpty(defaultIndex))
                throw new ArgumentNullException("configuration['elasticsearch: index']");

            connectionSettings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .EnableDebugMode()
                .ThrowExceptions(true)
                .DefaultFieldNameInferrer(b => b);

            AddDefaultMappings(connectionSettings);

            elasticClient = new ElasticClient(connectionSettings);

            CreateIndex(elasticClient);

        }


        public async Task SearchUsersAsync(string searchCriteria, int page = 1, int pageSize = 5)
        {
            await elasticClient.SearchAsync<User>(s =>
                    s.Query(q =>
                        q.QueryString(d =>
                            d.Query(searchCriteria)))
                                .From((page - 1) * pageSize));
        }

        public async Task AddUserAsync(User user)
        {
            await elasticClient.IndexDocumentAsync(user);
        }

        public async Task UpdatedUserAsync(User user)
        {
            await elasticClient.UpdateAsync<User>(user, u => u.Doc(user));
        }

        public async Task DeleteUserAsync(User user)
        {
            await elasticClient.DeleteAsync<User>(user);
        }

        public async Task GetAllUsersAsync()
        {

            var result = await elasticClient.SearchAsync<User>(s => s.Index("users").Size(10).Sort(q => q.Descending(p => p.Name)));
            var sdfd = result;
        }

        #region private methods

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<User>(userMapping => userMapping
                    .IndexName("users"));
        }

        private static void CreateIndex(IElasticClient client)
        {
            client.Indices.CreateAsync("users", creator => creator.Map<User>(user => user.AutoMap()));
        }

        public Task<IEnumerable<string>> SearchUsersAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
