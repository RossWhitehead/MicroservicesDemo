using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Data
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime LastUpdated { get; set; }
        public OrderStatus Status { get; set; }
    }
}
