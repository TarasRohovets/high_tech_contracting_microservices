using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Business.WebApi.Infrastructure.DatabaseExtension
{
    internal sealed class DatabaseBootstrap
    {
        public static void Configure(IConfiguration Configuration)
        {
            // Create DB if not created
            //var cs = Configuration.GetConnectionString("DC.BusinessDb");
            var cs_server = Configuration.GetConnectionString("DC.BusinessDbServer");
            try
            {
                MySqlConnectionStringBuilder sqlConnectionStringBuilder = new MySqlConnectionStringBuilder(cs_server);
                cs_server = sqlConnectionStringBuilder.ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(cs_server))
                {
                    //Thread.Sleep(40000);
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = DatabaseCreateScript.CheckDatabaseExists();
                    var exists = command.ExecuteNonQuery();

                    if (exists <= 0)
                    {
                        // connection.Open();
                        var command2 = connection.CreateCommand();
                        command2.CommandText = DatabaseCreateScript.CreateDatabase();
                        command2.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
