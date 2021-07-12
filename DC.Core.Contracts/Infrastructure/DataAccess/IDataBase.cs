using System;
using System.Data;

namespace DC.Core.Contracts.Infrastructure.DataAccess
{
    public interface IDataBase : IDisposable
    {
        IDbConnection DbConnection { get; }

        IDbConnection OpenConnection();
    }
}
