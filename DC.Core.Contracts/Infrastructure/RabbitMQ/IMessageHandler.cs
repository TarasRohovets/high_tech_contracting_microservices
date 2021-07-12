using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DC.Core.Contracts.Infrastructure.RabbitMQ
{
    public interface IMessageHandler
    {
        void Start(IMessageHandlerCallback callback);
        void Stop();
    }
}
