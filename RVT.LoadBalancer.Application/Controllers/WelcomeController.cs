using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RVT.Common.Models;
using RVT.LoadBalancer.Core;
using RVT.LoadBalancer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        public INode _node;
        public IMapper _mapper;
        public WelcomeController(IMapper mapper)
        {
            var bl = new BusinessManager();
            _node = bl.GetNodeActions();
            _mapper = mapper;
        }


        [HttpPost]
        public void Post([FromBody] Node node)
        {
           var participant = _mapper.Map<NodeData>(node);

            participant.IpAddress = node.Url;
            
            participant.RegisterDate = DateTime.Now;
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                AllowAutoRedirect = true,
            };

            participant.Sender = new HttpClient(handler);
            _node.RegisterNode(participant);           
        }



    }
}
