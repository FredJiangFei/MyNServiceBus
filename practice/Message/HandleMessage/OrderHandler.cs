using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace HandleMessage
{
    public class OrderHandler: IHandleMessages<Order>
    {
        public Task Handle(Order message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Order Id is {message.Id}");
            return Task.CompletedTask;
        }
    }
}
