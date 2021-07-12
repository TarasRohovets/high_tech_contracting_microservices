using DC.Core.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Business.Consumer.Email
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                                                        .SetBasePath(Directory.GetCurrentDirectory())
                                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                        .AddEnvironmentVariables()
                                                        .Build();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterRabbitMQPublisherHandler(hostContext.Configuration);
                    services.AddHostedService<EmailManager>();
                    services.AddLogging(builder =>
                    builder.AddDebug()
                       .AddConsole()
                       .AddConfiguration(Configuration.GetSection("Logging"))
                       .SetMinimumLevel(LogLevel.Information));
                });
    }
}
