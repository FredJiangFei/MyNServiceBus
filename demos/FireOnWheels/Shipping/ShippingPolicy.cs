using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Shipping
{
    public class ShippingPolicy : Saga<ShippingPolicyData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>
    {
        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Console.WriteLine($"OrderBilled {message.OrderId} - Should we ship now?");
            Data.IsOrderBilled = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Console.WriteLine($"OrderPlaced {message.OrderId} - Wait for payment");
            Data.IsOrderPlaced = true;
            var timeout = DateTime.UtcNow.AddSeconds(30);
            RequestTimeout<CancelOrder>(context, timeout).ConfigureAwait(false);
            return ProcessOrder(context);
        }

        protected override void ConfigureHowToFindSaga(
            SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(x => x.OrderId)
                .ToSaga(x => x.OrderId);
            mapper.ConfigureMapping<OrderBilled>(x => x.OrderId)
                .ToSaga(x => x.OrderId);
        }

        public Task Timeout(CancelOrder state, IMessageHandlerContext context)
        {
            Console.WriteLine($"CompleteOrder not received soon enough OrderId {Data.OrderId}. Calling MarkAsComplete");
            MarkAsComplete();
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new ShipOrder
                {
                    OrderId = Data.OrderId
                });
                MarkAsComplete();
            }
        }
    }
}
