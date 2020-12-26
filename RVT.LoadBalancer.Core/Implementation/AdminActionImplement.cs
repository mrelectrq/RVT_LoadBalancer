using Newtonsoft.Json;
using RVT.Common.Messages;
using RVT.Common.Models;
using RVT.Common.Responses;
using RVT.LoadBalancer.Core.ConsensusHandler;
using RVT.LoadBalancer.Core.Interfaces;
using RVT.LoadBalancer.Core.Mapper;
using RVT.LoadBalancer.Core.Storage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RVT.LoadBalancer.Core.Implementation
{
    public class AdminActionImplement : IAdministrator
    {
        public NodeRegResponse RegistrationAction(RegistrationMessage message)
        {

            var nodeMessage = new NodeRegMessage { Message = message };

            NodeData worker;
            var neighbors = Distributor.FormateNodeList(3, out worker);
            nodeMessage.NeighBours = neighbors;

            var serializedMessage =  JsonConvert.SerializeObject(nodeMessage);
            var content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");

            try
            {
                var response = worker.Sender.PostAsync(new Uri(new Uri(worker.IpAddress), "api/Register"), content).Result.Content.ReadAsStringAsync();
                var responseFromNode = JsonConvert.DeserializeObject<NodeRegResponse>(response.Result);
                return responseFromNode;
            }
            catch(AggregateException e)
            {
                return new NodeRegResponse { Status = false, Message = "Eroare de connectare la server LB:" + e.InnerException.ToString(), ProcessedTime = DateTime.Now };
            }

            
        }

        public string VoteAction(ChooserLbMessage message)
        {

            var _storage = NodeStorage.GetInstance();
            NodeData worker;
            var selected_Nodes = Distributor.FormateNodeList(3, out worker);
            var neighbors = Mapping.Mapper.Map<List<NodeNeighbor>>(selected_Nodes);

            var request_data = new NodeVoteMessage()
            {
                Message = message,
                Neighbors = neighbors
            };

            var serialized = JsonConvert.SerializeObject(request_data);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            try
            {
                var response = worker.Sender.PostAsync(new Uri(new Uri(worker.IpAddress), "api/Vote"), content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch(Exception e)
            {
                return "Error connection to Node" + e.Message;
            }
        }
    }
}
