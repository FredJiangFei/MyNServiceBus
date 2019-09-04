using System;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace CalculatePrice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // https://docs.particular.net/nservicebus/messaging/callbacks
            
            var config = new EndpointConfiguration("CalculatePrice");
            config.EnableCallbacks();
            config.MakeInstanceUniquelyAddressable("uniqueId");

            var transport = config.UseTransport<LearningTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PriceRequest), "Calculator");

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
                options.SetDestination("Calculator");

                // NServiceBus.Callbacks
                var request = new PriceRequest(5);
                var priceResponse = await endpoint
                    .Request<PriceResponse>(request, options);

                Console.WriteLine($"Price is {priceResponse.Price}");
            }
        }
    }
}
