using System;

namespace Messages
{
    public class OrderCreated
    {
        public int SagaId { get; set; }

        public DateTime LastUpdated { get; set; }
        public OrderStatus Status { get; set; }
    }
}
