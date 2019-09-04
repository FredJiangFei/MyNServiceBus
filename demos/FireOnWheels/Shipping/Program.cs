using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Shipping
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Shipping";

            var config= new EndpointConfiguration("Shipping");
            config.UseTransport<LearningTransport>();
            config.UsePersistence<LearningPersistence>();

            var endpoint = await Endpoint.Start(config)
                .ConfigureAwait(false);

            Console.ReadLine();

            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
