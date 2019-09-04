using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace Order
{
    public class ProcessOrderHandler : IHandleMessages<ProcessOrderCommand>
    {
        public async Task Handle(ProcessOrderCommand message,
            IMessageHandlerContext context)
        {
            Console.WriteLine($"Order Id is {message.OrderId}");

            Thread.Sleep(1000);
            await context.Publish<IOrderProcessedEvent>(e =>
            {
               e.OrderId = message.OrderId;
            });
        }
    }
}
