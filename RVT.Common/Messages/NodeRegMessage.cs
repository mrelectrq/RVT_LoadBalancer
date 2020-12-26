using RVT.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.Common.Messages
{
    public class NodeRegMessage
    {
        public RegistrationMessage Message { get; set; }
        public List<NodeNeighbor> NeighBours { get; set; }
    }
}
