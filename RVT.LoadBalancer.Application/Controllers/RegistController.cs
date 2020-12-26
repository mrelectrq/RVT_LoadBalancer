using Microsoft.AspNetCore.Mvc;
using RVT.Common.Messages;
using RVT.Common.Responses;
using RVT.LoadBalancer.Core;
using RVT.LoadBalancer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RegistController
    {
        public IAdministrator _admin;   

        public RegistController()
        {
            _admin = new BusinessManager().GetAdminActions();
        }

        [HttpPost]
        public ActionResult<NodeRegResponse> Register([FromBody] RegistrationMessage message)
        {
            var status = _admin.RegistrationAction(message);
            return status;
        }
    }
}
