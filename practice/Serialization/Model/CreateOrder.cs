using System;
using NServiceBus;

namespace Model
{
    public class CreateOrder: IMessage
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
    }
}
