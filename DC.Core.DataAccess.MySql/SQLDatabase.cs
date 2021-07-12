using System;
using System.Data;
using DC.Core.Contracts.Infrastructure.DataAccess;
using MySql.Data.MySqlClient;

namespace DC.Core.DataAccess.MySql
{
        public class SQLDatabase : IDataBase
        {
            private IDbConnection dbConnection;

            public IDbConnection DbConnection
            {
                get
                {
                    if (dbConnection == null || dbConnection.State == ConnectionState.Closed)
                    {
                        dbConnection = OpenConnection();
                    }
                    return dbConnection;
                }
            }

            private string _connectionString;

            public SQLDatabase(string connectionString)
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new System.ArgumentException("Empty connection string", nameof(connectionString));
                _connectionString = connectionString;
            }

            public IDbConnection OpenConnection()
            {
                MySqlConnectionStringBuilder sqlConnectionStringBuilder = new MySqlConnectionStringBuilder(_connectionString);

                _connectionString = sqlConnectionStringBuilder.ConnectionString;

                MySqlConnection connection = new MySqlConnection(_connectionString);
                connection.Open();

                return connection;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool dummy) => dbConnection?.Dispose();
        }
}
