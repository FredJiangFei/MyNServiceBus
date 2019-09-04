using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NServiceBus;

namespace Message
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

                await endpoint.Send("Order", new Order())
                    .ConfigureAwait(false);
            }
        }
    }
}
