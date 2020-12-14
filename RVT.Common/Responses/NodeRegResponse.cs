using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.Common.Responses
{
   public  class NodeRegResponse
    {
        public string IDVN { get; set; }
        public string VnPassword { get; set; }
        public string IDNP { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
