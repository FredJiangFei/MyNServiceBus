using System;
using NServiceBus;

namespace Shipping.Saga
{
    public class SagsData : ContainSagaData
    {
        public Guid OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }
}
