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
            Console.WriteLine($"Order {message.OrderId}, price ${message.Price}");

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId,
                Price = message.Price
            };
            return context.Publish(orderPlaced);
        }
    }
}