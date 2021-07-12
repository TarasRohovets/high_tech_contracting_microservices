using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DC.Business.Domain.Entities.Organization;

namespace DC.Business.Domain.Repositories.ElasticSearch
{
    public interface IUsersElasticRepository
    {
        Task<IEnumerable<string>> SearchUsersAsync(string searchCriteria);
        Task AddUserAsync(User user);
        Task GetAllUsersAsync();
        Task UpdatedUserAsync(User user);
    }
}
