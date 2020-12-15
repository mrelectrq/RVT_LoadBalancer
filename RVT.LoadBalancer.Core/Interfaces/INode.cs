using RVT.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.LoadBalancer.Core.Interfaces
{
    public interface INode
    {
        public void RegisterNode(NodeData node);
    }
}
