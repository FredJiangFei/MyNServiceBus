using System;
using NServiceBus;

namespace Messages
{
    public class PlaceOrder: IMessage
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public int Price { get; set; }
    }
}
