using System;
using Messages;
using NServiceBus;
using Order.CustomBehavior;

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

            // TimeToBeReceived
            var conventions = config.Conventions();
            conventions.DefiningTimeToBeReceivedAs(type =>
                {
                    if (type == typeof(PlaceOrder)) return TimeSpan.FromMinutes(1);

                    return TimeSpan.MaxValue;
                });

            // 对Message做一份备份 发送到audit
            config.AuditProcessedMessagesTo("audit");
            config.Pipeline.Register(typeof(CustomAuditDataBehavior), "Manipulates incoming headers");

            // 
            config.PurgeOnStartup(true);

            var result = Endpoint.Start(config).Result;
            Console.ReadKey();
        }
    }
}
