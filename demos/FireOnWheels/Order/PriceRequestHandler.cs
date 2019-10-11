using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Order.Helper;

namespace Order
{
    public class PriceRequestHandler : IHandleMessages<PriceRequest>
    {
        public async Task Handle(PriceRequest message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Message Id is {context.MessageId}");

            await context.Reply(new PriceResponse
            {
                Price = PriceCalculator.GetPrice(message)
            }).ConfigureAwait(false);
        }
    }
}
