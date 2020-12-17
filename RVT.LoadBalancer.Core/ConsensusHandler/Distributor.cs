using RVT.Common.Models;
using RVT.LoadBalancer.Core.Mapper;
using RVT.LoadBalancer.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RVT.LoadBalancer.Core.ConsensusHandler
{
    public static class Distributor
    { 
        

        public static List<NodeNeighbor> FormateNodeList(int i,out NodeData Executor)
        {
            var _storage = NodeStorage.GetInstance();
            var totalNodeList = _storage.GetNodes();
            var random = new Random();
            
            if(totalNodeList.Count()<=i)
            {
                throw new ArgumentOutOfRangeException("Number of registered Nodes is smaller than counter i");
            }
            else
            {
                var choosedIndex = random.Next(totalNodeList.Count());
                var choosedNode = totalNodeList.ElementAt(choosedIndex);
                Executor = choosedNode;
                var choosedNodes = totalNodeList.OrderBy(x => random.Next()).Where(m => m.NodeId != choosedNode.NodeId).Take(i).Distinct();
                var neighbours = Mapping.Mapper.Map<List<NodeNeighbor>>(choosedNodes);
                return neighbours;
            }



            
            // To think about how to send NodeList to Executor without HttpClient. public class Neighbour.... List<Neighbour> Neighbours
        }

    }
}
