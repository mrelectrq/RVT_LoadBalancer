using RVT.Common.Models;
using RVT.LoadBalancer.Core.Interfaces;
using RVT.LoadBalancer.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RVT.LoadBalancer.Core.Implementation
{
    public class NodeActionImplement : INode
    {
        public void RegisterNode(NodeData node)
        {
            var storage = NodeStorage.GetInstance();

            var registered_nodes = storage.GetNodes();

            if (registered_nodes.FirstOrDefault(m => m.NodeId==node.NodeId)==null)
            {
                storage.AddNode(node);
            }
            else
            {
                storage.ExcludeNode(node.NodeId);
                storage.AddNode(node);
            }

        }
    }
}
