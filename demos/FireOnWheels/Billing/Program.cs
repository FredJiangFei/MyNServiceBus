using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Billing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Billing";

            var config = new EndpointConfiguration("Billing");
            config.UseTransport<LearningTransport>();

            var endpoint = await Endpoint.Start(config)
                .ConfigureAwait(false);

            Console.ReadLine();

            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
