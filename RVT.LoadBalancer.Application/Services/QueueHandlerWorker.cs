using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services
{
    public class QueueHandlerWorker : IDisposable
    {
        private string _queueName;
        private readonly IQueueConnection _queueConnection;
        private IModel _receiverChannel;

        public QueueHandlerWorker(IQueueConnection connection,string queueName)
        {
            _queueConnection = connection;
            _queueName = queueName;
        }


        public void InitReceiverChannel()
        {

            if(!_queueConnection.IsConnected)
            {
                _queueConnection.TryConnect();
            }

            var channel = _queueConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName,
                exclusive: false,
                durable: false,
                autoDelete: false,
                arguments: null);


            var receiver = new EventingBasicConsumer(channel);

            receiver.Received += ReceivedEvent;

            
        }

        public void ReceivedEvent(object sender, BasicDeliverEventArgs args)
        {

        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
