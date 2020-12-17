using Microsoft.VisualStudio.TestTools.UnitTesting;
using RVT.Common.Models;
using RVT.LoadBalancer.Core.ConsensusHandler;
using RVT.LoadBalancer.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Core.ConsensusHandler.Tests
{
    [TestClass()]
    public class DistributorTests
    {
        [TestMethod()]
        public void FormateNodeListTest()
        {
            List<NodeData> node_list = new List<NodeData>
            {
                new NodeData(){NodeId="N00-1",Name="Node1"},
                new NodeData(){NodeId="N00-2",Name="Node2"},
                new NodeData(){NodeId="N00-3",Name="Node3"},
                new NodeData(){NodeId="N00-4",Name="Node4"},
                new NodeData(){NodeId="N00-5",Name="Node5"},
                new NodeData(){NodeId="N00-6",Name="Node6"},
                new NodeData(){NodeId="N00-7",Name="Node7"},
            };

            var _storage = NodeStorage.GetInstance();


            foreach(var item in node_list )
            {
                _storage.AddNode(item);
            }

            NodeData selectedNode;
            var selected_nodes = Distributor.FormateNodeList(3, out selectedNode);

            var outputList = Mapper.Mapping.Mapper.Map<List<NodeNeighbor>>(selected_nodes);

            Assert.IsNotNull(outputList);



        }
    }
}