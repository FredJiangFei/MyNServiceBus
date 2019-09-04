using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Order
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration("Order");
            config.UseTransport<LearningTransport>();
          
            var endpoint = await Endpoint.Start(config)
                .ConfigureAwait(false);
            Console.ReadLine();
            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
