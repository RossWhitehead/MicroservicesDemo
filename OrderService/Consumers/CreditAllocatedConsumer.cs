using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Consumers
{
    public class CreditAllocatedConsumer : IConsumer<CreditAllocated>
    {
        public Task Consume(ConsumeContext<CreditAllocated> context)
        {
            throw new NotImplementedException();
        }
    }
}
