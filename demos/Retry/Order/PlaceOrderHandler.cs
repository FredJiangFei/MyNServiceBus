using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Order.Exceptions;

namespace Order
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            throw new CustomerException();
        }
    }
}