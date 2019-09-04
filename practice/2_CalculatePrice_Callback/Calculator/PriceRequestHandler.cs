using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace Calculator
{
    public class PriceRequestHandler : IHandleMessages<PriceRequest>
    {
        public async Task Handle(PriceRequest message, IMessageHandlerContext context)
        {
            var price = message.Weight * 3;
            Console.WriteLine($"Send response price {price}");
            await context.Reply(new PriceResponse(price));
        }
    }
}
