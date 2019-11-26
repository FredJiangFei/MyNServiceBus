using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Calculator
{
    public class PriceRequestHandler : IHandleMessages<PriceRequest>
    {
        public async Task Handle(PriceRequest message, IMessageHandlerContext context)
        {
            await context.Reply(new PriceResponse
            {
                Price = message.Weight * 3
            }).ConfigureAwait(false);
        }
    }
}