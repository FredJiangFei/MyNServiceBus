using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Order
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            Console.Write(message.OrderId);
            return Task.CompletedTask;
        }
    }
}