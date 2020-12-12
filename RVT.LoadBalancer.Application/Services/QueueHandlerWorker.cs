using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services
{
    public class QueueHandlerWorker : IDisposable
    {

        public QueueHandlerWorker(IQueueConnection connection,string queueName)
        {

        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
