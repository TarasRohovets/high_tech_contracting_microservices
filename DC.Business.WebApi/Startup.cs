using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Business.WebApi.Helpers;
using DC.Business.WebApi.Infrastructure.Documentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Newtonsoft.Json;
using DC.Core.Contracts.Infrastructure.RabbitMQ;
using DC.Business.WebApi.Middleware;
using MySql.Data.MySqlClient;
using DC.Core.DataAccess.MySql;
using DC.Business.WebApi.Infrastructure;
using System.Threading;
using DC.Business.WebApi.Infrastructure.DatabaseExtension;
// using System.Text.Json.Serialization;

namespace DC.Business.WebApi
{
    internal class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                   // .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
                   // .AddXmlSerializerFormatters()
                   .SetCompatibilityVersion(CompatibilityVersion.Latest).AddNewtonsoftJson(o =>
                   {
                       // o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                       o.SerializerSettings.ContractResolver = new DefaultContractResolver();
                   });
            services.AddLogging(builder =>
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddConfiguration(Configuration.GetSection("Logging"))
                    .SetMinimumLevel(LogLevel.Information)
            );

            //services.AddMvc(option => option.EnableEndpointRouting = false)
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //services.AddControllers().AddNewtonsoftJson(options =>
            //         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //  );



            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAntiforgery();
            services.AddAutoMapper(typeof(Startup));
            // Configure Swagger
            SwaggerService.Configure(services);

            //// Register Dependencies
            new DependencyInjection().Bootstrap(services, Configuration);

            DatabaseBootstrap.Configure(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            app.UseHsts();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<UnhandledExceptionMiddleware>();

#if !DEBUG
            app.UseHttpsRedirection();
#endif
            app.UseCors(options =>
            {
                // options.WithOrigins("http://localhost:4200");
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            hostApplicationLifetime.ApplicationStarted.Register(() => { });
            //hostApplicationLifetime.ApplicationStopping.Register(() =>
            //{
            //    var rabbitMqClient = app.ApplicationServices.GetRequiredService<IRabbitMQClient>();
            //    rabbitMqClient.CloseConnection();
            //});

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(this.Configuration["VirtualDirectory"] + "../swagger/v1/swagger.json", "DC Server Api");
                c.RoutePrefix = "api";
                c.DefaultModelsExpandDepth(-1);
                //c.DocExpansion(DocExpansion.None);
            });
            app.UseMvc();
        }

    }
}
