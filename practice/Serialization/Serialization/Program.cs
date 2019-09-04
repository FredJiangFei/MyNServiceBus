using System;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.MessageMutator;

namespace Serialization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration("ClientUI");
            config.UseTransport<LearningTransport>();
            //var settings = new JsonSerializerSettings
            //{
            //    Formatting = Formatting.Indented
            //};
            //var serialization = config.UseSerialization<NewtonsoftSerializer>();
            //serialization.Settings(settings);
            //config.RegisterComponents(
            //    registration: components =>
            //    {
            //        components.ConfigureComponent<MessageBodyWriter>(DependencyLifecycle.InstancePerCall);
            //    });

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
                var person = new CreateOrder
                {
                   OrderId = 1,
                   CustomerId = 12,
                   Date = DateTime.Now
                };
                await endpoint.Send("Order",person)
                    .ConfigureAwait(false);
            }
        }
    }
}
