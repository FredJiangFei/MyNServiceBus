using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace OrderProcessed
{
    public class OrderProcessedEventHandler : IHandleMessages<IOrderProcessedEvent>
    {
        public Task Handle(IOrderProcessedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Order {message.OrderId} Processed");
            return Task.CompletedTask;
        }
    }
}
