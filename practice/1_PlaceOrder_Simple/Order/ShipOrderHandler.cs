using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace Order
{
    public class ShipOrderHandler : IHandleMessages<ShipOrderCommand>
    {
        public Task Handle(ShipOrderCommand message, 
            IMessageHandlerContext context)
        {
            Console.WriteLine($"Order Id is {message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
