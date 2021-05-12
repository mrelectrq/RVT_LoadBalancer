using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.Common.Models
{
    public class Node
    {
        public string Name { get; set; }
        public string NodeId { get; set; }
        public string SoftwareVersion { get; set; }
        public byte[] PublicKey { get; set; }
        public string Url { get; set; }
    }
}
