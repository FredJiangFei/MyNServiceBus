using System;
using NServiceBus;
using NServiceBus.Transport;
using Order.Exceptions;

namespace Order
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new EndpointConfiguration("Order");
            config.UseSerialization<NewtonsoftSerializer>();
            config.EnableInstallers();

            var transport = config.UseTransport<RabbitMQTransport>().UseConventionalRoutingTopology();
            transport.ConnectionString("host=127.0.0.1;username=admin;password=1qaz2wsx3edc4rfv;virtualhost=rabbitmq_vhost;");

            config.SendFailedMessagesTo("targetErrorQueue");
            ConfigRecover(config);

            var result = Endpoint.Start(config).Result;
            Console.ReadKey();
        }

        private static void ConfigRecover(EndpointConfiguration config)
        {
            var recover = config.Recoverability();

            // 对于UnrecoverableException，跳过重试
            recover.AddUnrecoverableException<UnrecoverableException>();

            // 自定义重试
            recover.CustomPolicy(CustomerExceptionPolicy);

            // 修改error message header
            recover.Failed(failed =>
            {
                failed.HeaderCustomization(headers =>
                {
                    if (headers.ContainsKey("NServiceBus.ExceptionInfo.Message"))
                    {
                        headers["NServiceBus.ExceptionInfo.Message"] = "message override";
                    }
                });
            });

            // 修改立即重试次数
            recover.Immediate(i => i.NumberOfRetries(3));

            // 修改延迟重试次数
            recover.Delayed(delayed =>
                {
                    delayed.NumberOfRetries(2);
                    // 延迟重试递增的时间
                    delayed.TimeIncrease(TimeSpan.FromSeconds(5));
                });
        }

        private static RecoverabilityAction CustomerExceptionPolicy(RecoverabilityConfig config, ErrorContext context)
        {
            var unrecover = context.Exception is UnrecoverableException;
            var customer = context.Exception is CustomerException;

            if (!unrecover
                && context.ImmediateProcessingFailures < config.Immediate.MaxNumberOfRetries)
            {
                Console.WriteLine($"DelayedRetry {context.DelayedDeliveriesPerformed}, ImmediateRetry {context.ImmediateProcessingFailures}");
                return RecoverabilityAction.ImmediateRetry();
            }

            if (!unrecover
                && context.ImmediateProcessingFailures == config.Immediate.MaxNumberOfRetries
                && context.DelayedDeliveriesPerformed < config.Delayed.MaxNumberOfRetries)
            {
                Console.WriteLine($"DelayedRetry {context.DelayedDeliveriesPerformed}");
                return RecoverabilityAction.DelayedRetry(config.Delayed.TimeIncrease);
            }

            if (customer
                && context.DelayedDeliveriesPerformed == config.Delayed.MaxNumberOfRetries)
            {

                Console.WriteLine("Alert customer exception");
            }

            return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
        }
    }
}
