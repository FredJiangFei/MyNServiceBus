using System;
using NServiceBus;

namespace Model
{
    public class ShipOrderCommand : IMessage
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
    }
}
