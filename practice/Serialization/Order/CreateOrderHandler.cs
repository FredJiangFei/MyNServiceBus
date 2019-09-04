using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace Order
{
    public class CreateOrderHandler : IHandleMessages<CreateOrder>
    {
        public Task Handle(CreateOrder message, 
            IMessageHandlerContext context)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
