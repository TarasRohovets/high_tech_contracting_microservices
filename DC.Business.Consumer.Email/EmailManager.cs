using DC.Business.Consumer.Email.Dtos;
using DC.Business.Consumer.Email.SendGrid;
using DC.Core.Contracts.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Business.Consumer.Email
{
    public class EmailManager : IHostedService, IMessageHandlerCallback // : BackgroundService
    {
        private readonly ILogger<EmailManager> _logger;
        //private ConnectionFactory _connectionFactory;
        //private IConnection _connection;
        //private IModel _channel;
        //private const string QueueName = "rabbitmq.emailworker";

        IMessageHandler _messageHandler;

        public EmailManager(IMessageHandler messageHandler, ILogger<EmailManager> logger)
        {
            _messageHandler = messageHandler;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
             _messageHandler.Start(this);
             return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string message)
        {
            try
            {
                var emailIcomingMessage = JsonSerializer.Deserialize<EmailDto>(message);
                _logger.LogInformation($"Sending #{emailIcomingMessage} confirmation email to [{emailIcomingMessage}].");

                // await Task.Delay(new Random().Next(1, 3) * 1000, stoppingToken); // simulate an async email process
                var emailService = new SendGridService();
                await emailService.Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while handling event.");
            }

            return true;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    stoppingToken.ThrowIfCancellationRequested();

        //    var messageCount = _channel.MessageCount(QueueName);
        //    if (messageCount > 0)
        //    {
        //        _logger.LogInformation($"\tDetected {messageCount} message(s).");
        //    }

        //    var consumer = new AsyncEventingBasicConsumer(_channel);
        //    consumer.Received += async (bc, ea) =>
        //    {
        //        if (ea.BasicProperties.UserId != "Administrator")
        //        {
        //            return;
        //        }
        //        var message = Encoding.UTF8.GetString(ea.Body.ToArray());

        //        try
        //        {
        //            var emailIcomingMessage = JsonSerializer.Deserialize<EmailDto>(message);
        //            _logger.LogInformation($"Sending #{emailIcomingMessage} confirmation email to [{emailIcomingMessage}].");

        //            // await Task.Delay(new Random().Next(1, 3) * 1000, stoppingToken); // simulate an async email process
        //            var emailService = new SendGridService();
        //            await emailService.Execute();

        //            _logger.LogInformation($"Order #{emailIcomingMessage} confirmation email sent.");
        //            _channel.BasicAck(ea.DeliveryTag, false);
        //        }
        //        catch (JsonException)
        //        {
        //            _logger.LogError($"JSON Parse Error: '{message}'.");
        //            _channel.BasicNack(ea.DeliveryTag, false, false);
        //        }
        //        catch (AlreadyClosedException)
        //        {
        //            _logger.LogInformation("RabbitMQ is closed!");
        //        }
        //        catch (Exception e)
        //        {
        //            _logger.LogError(default, e, e.Message);
        //        }
        //    };

        //    _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

        //    await Task.CompletedTask;
        //}

        //public EmailManager(ILogger<EmailManager> logger)
        //{
        //    _logger = logger;
        //}

        //public override Task StartAsync(CancellationToken cancellationToken)
        //{
        //    var rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");

        //    ConnectionFactory _connectionFactory = new ConnectionFactory();
        //    _connectionFactory.UserName = "Administrator";
        //    _connectionFactory.Password = "Password";
        //    _connectionFactory.VirtualHost = "/";
        //    _connectionFactory.Ssl.Enabled = false;
        //    _connectionFactory.HostName = "rabbitmq1"; //host.docker.internal
        //    _connectionFactory.Port = 5672;
        //    _connectionFactory.DispatchConsumersAsync = true;

        //    _connection = _connectionFactory.CreateConnection();

        //    _channel = _connection.CreateModel();
        //    _channel.ExchangeDeclare("blya", ExchangeType.Fanout, true);
        //    _channel.QueueDeclare(QueueName, false, false, false, null);
        //    _channel.QueueBind(QueueName, "blya", "");
        //    _channel.BasicQos(0, 1, false);
        //    _logger.LogInformation($"Queue [{QueueName}] is waiting for messages.");

        //    return base.StartAsync(cancellationToken);
        //}

        //public override async Task StopAsync(CancellationToken cancellationToken)
        //{
        //    await base.StopAsync(cancellationToken);
        //    _connection.Close();
        //    _logger.LogInformation("RabbitMQ connection is closed.");
        //}
    }
}
