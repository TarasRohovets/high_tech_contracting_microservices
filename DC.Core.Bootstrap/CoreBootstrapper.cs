using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DC.Core.Contracts.Infrastructure.Security;
using DC.Core.Security.Jwt;
using DC.Core.Contracts.Infrastructure.RabbitMQ;
using DC.Core.Messaging.RabbitMQ;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace DC.Core.Bootstrap
{
    public static class CoreBootstrapper
    {
        #region Private properties RabbitMq

        private const int DEFAULT_PORT = 5672;
        private static string _host;
        private static string _userName;
        private static string _password;
        private static string _exchange;
        private static string _queue;
        private static string _routingKey;
        private static int _port;
        private static List<string> _errors;
        private static bool _isValid;

        #endregion

        public static IServiceCollection RegisterJwtTokenServices(this IServiceCollection serviceContainer, IConfiguration configuration)
        {
           // if (string.IsNullOrWhiteSpace(configuration["JWT:Audience"]))
           //     throw new Exception("JWT:Audience");
           // if (string.IsNullOrWhiteSpace(configuration["JWT:Issuer"]))
           //     throw new Exception("JWT:Issuer");
            if (string.IsNullOrWhiteSpace(configuration["JWT:SecretKey"]))
                throw new Exception("JWT:SecretKey");
            if (string.IsNullOrWhiteSpace(configuration["JWT:TimeToLive"]))
                throw new Exception("JWT:TimeToLive");

            serviceContainer.AddSingleton<ITokenService, JwtTokenService>(serviceProvider => new JwtTokenService(configuration["JWT:Audience"],
                                                                                                   configuration["JWT:Issuer"],
                                                                                                   configuration["JWT:SecretKey"],
                                                                                                   Convert.ToInt32(configuration["JWT:TimeToLive"])));
            return serviceContainer;
        }

        #region RabbitMQSection

        public static void RegisterRabbitMQPublisherHandler(this IServiceCollection services, IConfiguration config)
        {
            GetRabbitMQSettings(config, "RabbitMQHandler");
            services.AddTransient<IMessageHandler>(_ => new RabbitMQHandler(
                _host, _userName, _password, _exchange, _queue, _routingKey, _port));
        }

        public static IServiceCollection RegisterRabbitMQPublisher(this IServiceCollection serviceContainer, IConfiguration configuration)
        {
            GetRabbitMQSettings(configuration, "RabbitMQPublisher");
            serviceContainer.AddTransient<IRabbitMQClient>(_ => new RabbitMQClient(
                _host, _userName, _password, _exchange, _port));

            //ConnectionFactory _connectionFactory = new ConnectionFactory();
            //_connectionFactory.UserName = "Administrator";
            //_connectionFactory.Password = "Password";
            //_connectionFactory.VirtualHost = "/";
            //_connectionFactory.Ssl.Enabled = false;
            //_connectionFactory.HostName = "rabbitmq1"; //host.docker.internal
            //_connectionFactory.Port = 5672;

            //var _connection = _connectionFactory.CreateConnection();

            //serviceContainer.AddSingleton(_connection);
            //serviceContainer.AddSingleton<IRabbitMQClient, RabbitMQClient>();

            return serviceContainer;
        }


        private static void GetRabbitMQSettings(IConfiguration config, string sectionName)
        {
            _isValid = true;
            _errors = new List<string>();

            var configSection = config.GetSection(sectionName);
            if (!configSection.Exists())
            {
                throw new Exception($"Required config-section '{sectionName}' not found.");
            }

            // get configuration settings
            DetermineHost(configSection);
            DeterminePort(configSection);
            DetermineUsername(configSection);
            DeterminePassword(configSection);
            DetermineExchange(configSection);
            if (sectionName == "RabbitMQHandler")
            {
                DetermineQueue(configSection);
                DetermineRoutingKey(configSection);
            }

            // handle possible errors
            if (!_isValid)
            {
                var errorMessage = new StringBuilder("Invalid RabbitMQ configuration:");
                _errors.ForEach(e => errorMessage.AppendLine(e));
                throw new Exception(errorMessage.ToString());
            }
        }

        private static void DetermineHost(IConfigurationSection configSection)
        {
            _host = configSection["Host"];
            if (string.IsNullOrEmpty(_host))
            {
                _errors.Add("Required config-setting 'Host' not found.");
                _isValid = false;
            }
        }

        private static void DeterminePort(IConfigurationSection configSection)
        {
            string portSetting = configSection["Port"];
            if (string.IsNullOrEmpty(portSetting))
            {
                _port = DEFAULT_PORT;
            }
            else
            {
                if (int.TryParse(portSetting, out int result))
                {
                    _port = result;
                }
                else
                {
                    _isValid = false;
                    _errors.Add("Unable to parse config-setting 'Port' into an integer.");
                }
            }
        }

        private static void DetermineUsername(IConfigurationSection configSection)
        {
            _userName = configSection["UserName"];
            if (string.IsNullOrEmpty(_userName))
            {
                _isValid = false;
                _errors.Add("Required config-setting 'UserName' not found.");
            }
        }

        private static void DeterminePassword(IConfigurationSection configSection)
        {
            _password = configSection["Password"];
            if (string.IsNullOrEmpty(_password))
            {
                _isValid = false;
                _errors.Add("Required config-setting 'Password' not found.");
            }
        }

        private static void DetermineExchange(IConfigurationSection configSection)
        {
            _exchange = configSection["Exchange"];
            if (string.IsNullOrEmpty(_exchange))
            {
                _isValid = false;
                _errors.Add("Required config-setting 'Exchange' not found.");
            }
        }

        private static void DetermineQueue(IConfigurationSection configSection)
        {
            _queue = configSection["Queue"];
            if (string.IsNullOrEmpty(_queue))
            {
                _isValid = false;
                _errors.Add("Required config-setting 'Queue' not found.");
            }
        }

        private static void DetermineRoutingKey(IConfigurationSection configSection)
        {
            _routingKey = configSection["RoutingKey"] ?? "";
        }

        #endregion


    }
}
