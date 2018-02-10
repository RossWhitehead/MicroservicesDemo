using Common.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Consumers
{
    public class CreditAllocationFailedConsumer : IConsumer<CreditAllocationFailed>
    {
        public Task Consume(ConsumeContext<CreditAllocationFailed> context)
        {
            throw new NotImplementedException();
        }
    }
}
