using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Shipping
{
    public class ShippingPolicyData : ContainSagaData
    {
        public Guid OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }
}
