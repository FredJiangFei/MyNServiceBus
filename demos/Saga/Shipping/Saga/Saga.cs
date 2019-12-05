using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Sagas;

namespace Shipping.Saga
{
    public class Saga : Saga<SagsData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<CancelOrder>,
        IHandleSagaNotFound
    // IFindSagas<MySagaData>.Using<MyMessage> // 自定义找寻saga逻辑
    {

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagsData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(x => x.OrderId)
                .ToSaga(x => x.OrderId);
            mapper.ConfigureMapping<OrderBilled>(x => x.OrderId)
                .ToSaga(x => x.OrderId);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Console.WriteLine($"OrderBilled {message.OrderId} - Should we ship now?");
            Data.IsOrderBilled = true;

            //Thread.Sleep(5000);
            //context.Reply()

            return SendShipOrderCommand(context);
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Console.WriteLine($"OrderPlaced {message.OrderId} - Wait for payment");
            Data.IsOrderPlaced = true;

            var timeout = DateTime.UtcNow.AddSeconds(30);
            RequestTimeout<CancelOrder>(context, timeout).ConfigureAwait(false);

            return SendShipOrderCommand(context);
        }

        public Task Timeout(CancelOrder state, IMessageHandlerContext context)
        {
            Console.WriteLine($"Timeout {Data.OrderId}. Calling MarkAsComplete");
            MarkAsComplete();
            return Task.CompletedTask;
        }

        private async Task SendShipOrderCommand(IMessageHandlerContext context)
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

        // 没有找到saga
        public Task Handle(object message, IMessageProcessingContext context)
        {
            var sagaDisappearedMessage = new SagaDisappearedMessage();
            return context.Reply(sagaDisappearedMessage);
        }

        //public Task<MySagaData> FindBy(MyMessage message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
        //{

        //}
    }

    public class SagaDisappearedMessage
    {

    }
}
