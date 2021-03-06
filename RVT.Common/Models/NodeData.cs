﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RVT.Common.Models
{
    public class NodeData
    {
        public string Name { get; set; }
        public string NodeId { get; set; }
        public string SoftwareVersion { get; set; }
        public byte[] PublicKey { get; set; }
        public DateTime RegisterDate { get; set; }
        public string IpAddress { get; set; }     
        public HttpClient Sender { get; set; }
    }
}
