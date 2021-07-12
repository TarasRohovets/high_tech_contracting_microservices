using DC.Core.Contracts.Infrastructure.RabbitMQ;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Core.Messaging.RabbitMQ
{
    public class RabbitMQClient : IRabbitMQClient, IDisposable
    {
        private const int DEFAULT_PORT = 5672;
        private readonly List<string> _hosts;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;
        private IConnection _connection;
        private IModel _model;


        //private readonly IConnection _connection;
        //private readonly IModel _channel;

        //public RabbitMQClient(IConnection connection)
        //{
        //    _connection = connection;
        //    _channel = _connection.CreateModel();
        //    _channel.ConfirmSelect();
        //}

        public RabbitMQClient(string host, string username, string password, string exchange, int port)
    : this(new List<string>() { host }, username, password, exchange, port)
        {
        }

        public RabbitMQClient(IEnumerable<string> hosts, string username, string password, string exchange, int port)
        {
            _hosts = new List<string>(hosts);
            _port = port;
            _username = username;
            _password = password;
            _exchange = exchange;

            Connect();
        }

        private void Connect()
        {
            Policy.
                Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
              .WaitAndRetry(200, r => TimeSpan.FromSeconds(60), (ex, ts) => { Console.WriteLine("Error connecting to RabbitMQ. Retrying in 60 sec."); })
                .Execute(() =>
                {
                    var factory = new ConnectionFactory() { UserName = _username, Password = _password, Port = _port };
                    factory.VirtualHost = "/";
                    //Console.WriteLine("writing hosts: " + _hosts[0]);
                    //Thread.Sleep(40000);
                    _connection = factory.CreateConnection(_hosts);
                    _model = _connection.CreateModel();
                    _model.ExchangeDeclare(_exchange, "fanout", durable: true, autoDelete: false);
                });
        }

        public Task PublishMessageAsync(string payload, string routingKey)
        {
            return Task.Run(() =>
            {
                var body = Encoding.UTF8.GetBytes(payload);
                IBasicProperties properties = _model.CreateBasicProperties();
                properties.AppId = "OrderApi";
                properties.Persistent = true;
                properties.UserId = "Administrator";
                properties.MessageId = Guid.NewGuid().ToString("N");
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                _model.BasicPublish(_exchange, routingKey, properties, body);
            });
        }

        public void Dispose()
        {
            _model?.Dispose();
            _model = null;
            _connection?.Dispose();
            _connection = null;
        }

        ~RabbitMQClient()
        {
            Dispose();
        }

        //public void Publish(string exchange, string routingKey, string payload)
        //{
        //    var props = _channel.CreateBasicProperties();
        //    props.AppId = "OrderApi";
        //    props.Persistent = true;
        //    props.UserId = "Administrator";
        //    props.MessageId = Guid.NewGuid().ToString("N");
        //    props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        //    var body = Encoding.UTF8.GetBytes(payload);
        //    _channel.BasicPublish(exchange, routingKey, props, body);
        //    _channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
        //}

        //public void CloseConnection()
        //{
        //    _connection?.Close();
        //}
    }
}
