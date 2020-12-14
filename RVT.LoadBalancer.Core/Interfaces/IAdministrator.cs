using RVT.Common.Messages;
using RVT.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace RVT.LoadBalancer.Core.Interfaces
{
    public  interface IAdministrator
    {
        void VoteAction(ChooserLbMessage message);

        public NodeRegResponse RegistrationAction(RegistrationMessage message);
    }
}
