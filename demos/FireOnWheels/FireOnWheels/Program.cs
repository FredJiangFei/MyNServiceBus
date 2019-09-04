using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace FireOnWheels
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration("ClientUI");
            config.EnableCallbacks();
            config.MakeInstanceUniquelyAddressable("uniqueId");

            var transport = config.UseTransport<LearningTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Order");

            var endpoint = await Endpoint.Start(config)
                .ConfigureAwait(false);

            await RunLoop(endpoint).ConfigureAwait(false);
            await endpoint.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpoint)
        {
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        await PlaceOrder(endpoint, 3);
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        await PlaceOrder(endpoint, 9);
                        break;
                }
            }
        }

        private static async Task PlaceOrder(
            IEndpointInstance endpoint,
            int weight)
        {
            var price = await GetPrice(endpoint, weight);

            await endpoint.Send(new PlaceOrder
            {
                Price = price
            }).ConfigureAwait(false);
        }

        private static async Task<int> GetPrice(IEndpointInstance endpoint, int weight)
        {
            var options = new SendOptions();
            options.SetDestination("Order");

            var request = new PriceRequest { Weight = weight };
            var priceResponse = await endpoint
                .Request<PriceResponse>(request, options);
            var price = priceResponse.Price;
            return price;
        }
    }
}
