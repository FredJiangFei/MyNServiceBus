using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Cart
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration("Cart");

            config.UseSerialization<NewtonsoftSerializer>();
            config.EnableInstallers();

            var transport = config.UseTransport<RabbitMQTransport>().UseConventionalRoutingTopology();
            transport.ConnectionString("host=127.0.0.1;username=admin;password=1qaz2wsx3edc4rfv;virtualhost=rabbitmq_vhost;");

            var endpoint = Endpoint.Start(config).Result;
            await SendOrder(endpoint);
        }

        static async Task SendOrder(IEndpointInstance endpoint)
        {
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                var order = new PlaceOrder
                {
                    Price = 1
                };

                await endpoint.Send("Order", order);
            }
        }
    }
}
