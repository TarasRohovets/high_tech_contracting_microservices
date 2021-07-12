using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Business.Application.Contracts.Interfaces.Services
{
    public interface IIndexService
    {
        Task IndexCities(string city1, string city2);
    }
}
