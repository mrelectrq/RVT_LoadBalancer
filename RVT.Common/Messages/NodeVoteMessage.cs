using RVT.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.Common.Messages
{
    public class NodeVoteMessage
    {
        public ChooserLbMessage messate { get; set; }
        public List<Node> Neighbours { get; set; }
    }
}
