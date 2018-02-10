using System;
using Common.Enums;

namespace Common.Messages
{
    public class OrderCreated
    {
        public Guid SagaId { get; set; }

        public DateTime LastUpdated { get; set; }
        public OrderStatus Status { get; set; }
    }
}
