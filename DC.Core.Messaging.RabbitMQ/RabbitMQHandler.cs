using DC.Core.Contracts.Infrastructure.RabbitMQ;
using MassTransit.RabbitMqTransport;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Core.Messaging.RabbitMQ
{
    public class RabbitMQHandler : IMessageHandler
    {
        private const int DEFAULT_PORT = 5672;
        private readonly List<string> _hosts;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;
        private readonly string _queuename;
        private readonly string _routingKey;
        private readonly int _port;
        private IConnection _connection;
        private IModel _model;
        private AsyncEventingBasicConsumer _consumer;
        private string _consumerTag;
        private IMessageHandlerCallback _callback;

        public RabbitMQHandler(string host, string username, string password, string exchange, string queuename, string routingKey, int port)
    : this(new List<string>() { host }, username, password, exchange, queuename, routingKey, port)
        {
        }

        public RabbitMQHandler(IEnumerable<string> hosts, string username, string password, string exchange, string queuename, string routingKey, int port)
        {
            _hosts = new List<string>(hosts);
            _port = port;
            _username = username;
            _password = password;
            _exchange = exchange;
            _queuename = queuename;
            _routingKey = routingKey;
        }

        public void Start(IMessageHandlerCallback callback)
        {
            _callback = callback;

            var retryPolicy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
              .WaitAndRetry(200, r => TimeSpan.FromSeconds(60), (ex, ts) => { Console.WriteLine("Error connecting to RabbitMQ. Retrying in 60 sec."); });

            try
            {
                retryPolicy.Execute(() =>
                {

                    var factory = new ConnectionFactory() { UserName = _username, Password = _password, DispatchConsumersAsync = true, Port = _port };
                    factory.VirtualHost = "/";
                    //Console.WriteLine("writing hosts: " + _hosts);
                    //Thread.Sleep(40000);
                    _connection = factory.CreateConnection(_hosts);
                    _model = _connection.CreateModel();
                    _model.ExchangeDeclare(_exchange, "fanout", durable: true, autoDelete: false);
                    _model.QueueDeclare(_queuename, durable: true, autoDelete: false, exclusive: false);
                    _model.QueueBind(_queuename, _exchange, _routingKey);
                    _consumer = new AsyncEventingBasicConsumer(_model);
                    _consumer.Received += Consumer_Received;
                    _consumerTag = _model.BasicConsume(_queuename, false, _consumer);

                });
            }
            catch
            {
                Console.WriteLine("exception here.");
            }




        }

        public void Stop()
        {
            _model.BasicCancel(_consumerTag);
            _model.Close(200, "Goodbye");
            _connection.Close();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            if (await HandleEvent(ea))
            {
                _model.BasicAck(ea.DeliveryTag, false);
            }
        }

        private Task<bool> HandleEvent(BasicDeliverEventArgs ea)
        {
            // get body
            string body = Encoding.UTF8.GetString(ea.Body.ToArray());

            // call callback to handle the message
            return _callback.HandleMessageAsync(body);
        }
    }
}
