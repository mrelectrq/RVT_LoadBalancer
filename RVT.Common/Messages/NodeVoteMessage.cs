using RVT.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.Common.Messages
{
    public class NodeVoteMessage
    {
        public ChooserLbMessage Message { get; set; }
        public List<NodeNeighbor> Neighbours { get; set; }
    }
}
