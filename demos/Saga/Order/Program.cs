using System;
using NServiceBus;

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

            var result = Endpoint.Start(config).Result;
            Console.ReadKey();
        }
    }
}
