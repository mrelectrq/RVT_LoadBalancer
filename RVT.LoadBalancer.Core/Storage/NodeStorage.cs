using RVT.Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RVT.LoadBalancer.Core.Storage
{
    public class NodeStorage
    {
        private static readonly NodeStorage _storage = new NodeStorage();

        private static ConcurrentDictionary<string, NodeData> Nodes;

        private NodeStorage()
        {
            Nodes = new ConcurrentDictionary<string, NodeData>();
        }

        public static NodeStorage GetInstance()
        {
            return _storage;
        }

        public void AddNode(NodeData node)
        {
            Nodes.TryAdd(node.NodeId, node);
        }

        public IEnumerable<NodeData> GetNodes()
        {
            var nodes = Nodes.Select(m => m.Value);
            return nodes;
        }
        
        public void ExcludeNode(string nodeId)
        {
            NodeData excludedNode;
           Nodes.TryRemove(nodeId,out excludedNode);
        }

    }
}
