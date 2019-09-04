﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Billing
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Received OrderPlaced, OrderId = {message.OrderId}, Price is {message.Price}, - Charging credit card...");
            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderBilled);
        }
    }
}