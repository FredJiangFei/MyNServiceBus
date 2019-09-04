using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace PlaceOrder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration("ClientUI");
            config.UseTransport<LearningTransport>();
            
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
                var options = new SendOptions();
                options.SetDestination("Order");
                await endpoint.Send(new ShipOrderCommand(), options)
                    .ConfigureAwait(false);
            }
        }
    }
}

