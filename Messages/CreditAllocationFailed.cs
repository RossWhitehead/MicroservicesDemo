using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class CreditAllocationFailed
    {
        public Guid SagaId { get; set; }

        public string FailureReason { get; set; }
    }
}
