using RVT.Common.Messages;
using RVT.Common.Responses;
using RVT.LoadBalancer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.LoadBalancer.Core.Implementation
{
    public class AdminActionImplement : IAdministrator
    {
        public NodeRegResponse RegistrationAction(RegistrationMessage message)
        {
            throw new NotImplementedException();
        }

        public void VoteAction(ChooserLbMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
