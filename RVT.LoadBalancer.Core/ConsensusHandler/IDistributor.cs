using RVT.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.LoadBalancer.Core.ConsensusHandler
{
    interface IDistributor
    {
        public NodeData Executor { get; set; }
        public void FormateNodeList(int i);
    }
}
