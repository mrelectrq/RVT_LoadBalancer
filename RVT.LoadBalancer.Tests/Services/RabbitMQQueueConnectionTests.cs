using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RVT.Common.Messages;
using RVT.Common.Models;
using RVT.LoadBalancer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services.Tests
{
    [TestClass()]
    public class RabbitMQQueueConnectionTests
    {

        [TestMethod()]
        public void RabbitMQCoonnectivityTest()
        {
            var clientFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "root",
                Password = "root",
                Port = 5672,
            };
            var message = new ChooserLbMessage()
            {
                //IDNP = "512412412312",
                IDVN = "1241231231",
                PartyChoosed = 3,
                Vote_date = DateTime.Now
            };


            var connection = clientFactory.CreateConnection();

            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "voteDataMsg", durable: false, exclusive: false, autoDelete: false, arguments: null);


                string messageSerialized = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageSerialized);

                channel.BasicPublish(exchange: "", routingKey: "voteDataMsg", basicProperties: null,
                    body: body);
            }


            var receive_connection = new RabbitMQQueueConnection(clientFactory);
            receive_connection.InitReceiverChannel();


            Assert.IsTrue(receive_connection.IsConnected);
        }

        [TestMethod()]
        public void RabbitMQResponsePublisher()
        {
            var response = "blavlbalba";
            var clientFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
            };

            var connection = clientFactory.CreateConnection();


            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "voteResponse", durable: false, exclusive: false, autoDelete: false, arguments: null);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                properties.Headers = new Dictionary<string, object>();


                string messageSerialized = JsonConvert.SerializeObject(response);
                var body = Encoding.UTF8.GetBytes(messageSerialized);

                channel.ConfirmSelect();

                channel.BasicPublish(exchange: "", routingKey: "voteResponse", basicProperties: properties,
                    body: body);

                channel.WaitForConfirmsOrDie();
                channel.ConfirmSelect();
            }

        }

}
}