using System;
using NServiceBus;

namespace Model
{
    public class ProcessOrderCommand : IMessage
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
    }
}
