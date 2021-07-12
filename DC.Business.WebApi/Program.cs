using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DC.Business.WebApi
{
    internal class Program
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

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>()
                   .UseConfiguration(Configuration);
                   //.UseKestrel(o =>
                   //{
                   //    o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(int.Parse(Configuration["Server:KeepAliveTimeout"]));
                   //    o.Limits.MaxRequestBodySize = null;
                   //});

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        // Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>()
        //        .UseConfiguration(Configuration);
        //    });

    }
}
