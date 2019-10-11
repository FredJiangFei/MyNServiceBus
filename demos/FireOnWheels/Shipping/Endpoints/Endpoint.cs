using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport;
using Shipping.Exceptions;

namespace Shipping.Endpoints
{
    public sealed class Endpoint
    {
        internal IEndpointInstance Instance { get; private set; }

        public async Task StartAsync()
        {
            var config = new EndpointConfiguration("Shipping");
            config.UseTransport<LearningTransport>();
            config.UsePersistence<LearningPersistence>();

            var recoverability = config.Recoverability();
            recoverability.AddUnrecoverableException<UnrecoverableException>();
            recoverability.CustomPolicy(CustomerExceptionPolicy);
            Instance = await NServiceBus.Endpoint.Start(config);
        }

        private static RecoverabilityAction CustomerExceptionPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var isUnrecoverableException = context.Exception is UnrecoverableException;
            var isCustomerException = context.Exception is CustomerException;

            if (!isUnrecoverableException
                && context.ImmediateProcessingFailures < config.Immediate.MaxNumberOfRetries)
            {
                return RecoverabilityAction.ImmediateRetry();
            }

            if (!isUnrecoverableException
                && context.ImmediateProcessingFailures == config.Immediate.MaxNumberOfRetries
                && context.DelayedDeliveriesPerformed < config.Delayed.MaxNumberOfRetries)
            {
                return RecoverabilityAction.DelayedRetry(config.Delayed.TimeIncrease);
            }

            if (isCustomerException
                && context.DelayedDeliveriesPerformed == config.Delayed.MaxNumberOfRetries)
            {

                Console.WriteLine("Alert customer exception");
            }

            return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
        }

        public async Task StopAsync()
        {
            if (Instance == null) return;
            await Instance.Stop();
            Instance = null;
        }
    }
}
