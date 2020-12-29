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

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: receiver); /// very IMPORTANT
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

                var response = _adminBL.VoteAction(message);

                PublishResponse(response,"voteResponse",args.BasicProperties.Headers);
            }
        }

        private void PublishResponse(string response,string _queueName, IDictionary<string,object> headers)
        {
            if(!_queueConnection.IsConnected)
            {
                _queueConnection.TryConnect();
            }

            using(var channel =_queueConnection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(response);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Headers = headers;
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                //channel.BasicAcks Acknowledge implementation in case if Broker received message
                channel.ConfirmSelect();
            }
        }

        public void Dispose()
        {
            if (_receiverChannel != null)
            {
                _receiverChannel.Dispose();
              //  _receiverChannel = InitReceiverChannel();
            }
        }
    }
}
