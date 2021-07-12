using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Core.Contracts.Infrastructure.RabbitMQ
{
    public interface IRabbitMQClient
    {
        //void Publish(string exchange, string routingKey, string payload);
        //void CloseConnection();

        Task PublishMessageAsync(string payload, string routingKey);
    }
}
