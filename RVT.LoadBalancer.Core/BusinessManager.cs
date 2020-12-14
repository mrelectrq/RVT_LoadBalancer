using RVT.LoadBalancer.Core.Implementation;
using RVT.LoadBalancer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.LoadBalancer.Core
{
    public class BusinessManager
    {
        public IAdministrator GetAdminActions()
        {
            return new AdminActionImplement();
        }

        public INode GetNodeActions()
        {
            return new NodeActionImplement();
        }
    }
}
