using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace OrderProcessed
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration("Web");
            var transport =config.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ProcessOrderCommand), "Order");

            var endpoint = await Endpoint.Start(config)
                .ConfigureAwait(false);

            await RunLoop(endpoint).ConfigureAwait(false);
            await endpoint.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpoint)
        {
            while (true)
            {
                Console.ReadKey();
                Console.WriteLine();
                await endpoint.Send(new ProcessOrderCommand())
                    .ConfigureAwait(false);
            }
        }
    }
}

