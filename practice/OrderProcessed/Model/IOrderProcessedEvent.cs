using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Model
{
    public interface IOrderProcessedEvent : IEvent
    {
        Guid OrderId { get; set; }
    }
}
