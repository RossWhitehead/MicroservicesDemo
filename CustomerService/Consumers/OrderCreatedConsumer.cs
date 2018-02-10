using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Common.Messages;
using Serilog;

namespace CustomerService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public OrderCreatedConsumer()
        {
        }

        public Task Consume(ConsumeContext<OrderCreated> context)
        {
            var creditAllocated = new CreditAllocated
            {
                SagaId = context.Message.SagaId               
            };

            context.Publish<CreditAllocated>(creditAllocated);

            return Task.CompletedTask;
        }
    }
}
