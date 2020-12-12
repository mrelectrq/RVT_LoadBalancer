using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services
{
    public interface IQueueConnection
    {
        bool IsConnected {get;}
        bool TryConnect();
        IModel CreateModel();
        void InitReceiverChannel();
        void Disconnect();
    }
}
