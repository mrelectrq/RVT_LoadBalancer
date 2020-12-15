using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RVT.Common.Messages;
using RVT.LoadBalancer.Core;
using RVT.LoadBalancer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services
{
    public class QueueHandlerWorker : IDisposable
    {
        private string _queueName;
        private readonly IQueueConnection _queueConnection;
        private IModel _receiverChannel;
        private readonly IAdministrator _adminBL;

        public QueueHandlerWorker(IQueueConnection connection,string queueName)
        {
            _queueConnection = connection;
            _queueName = queueName;
            _adminBL = new BusinessManager().GetAdminActions();
        }


        public IModel InitReceiverChannel()
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

            channel.CallbackException += (sender, args) =>
            {
                _receiverChannel.Dispose();
                _receiverChannel = InitReceiverChannel();
            };
            return channel;
        }

        public void ReceivedEvent(object sender, BasicDeliverEventArgs args)
        {
            if (args.RoutingKey == "voteDataMsg")
            {
                var data = Encoding.UTF8.GetString(args.Body.ToArray());
                ChooserLbMessage message = JsonConvert.DeserializeObject<ChooserLbMessage>(data);

                _adminBL.VoteAction(message);

            }
        }


        public void Dispose()
        {
            if (_receiverChannel != null)
            {
                _receiverChannel.Dispose();
                _receiverChannel = InitReceiverChannel();
            }
        }
    }
}
