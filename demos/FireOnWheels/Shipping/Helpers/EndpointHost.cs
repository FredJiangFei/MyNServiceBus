using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Endpoint = Shipping.Endpoints.Endpoint;

namespace Shipping.Helpers
{
    public static class EndpointHost
    {
        public static async Task RunAsConsoleAsync(Endpoint endpoint, Func<IMessageSession, Task> consoleHolder = null)
        {
            EnsureEndpoint(endpoint);
            endpoint.StartAsync().Wait();

            if (consoleHolder == null)
            {
                consoleHolder = instance => new HostBuilder().RunConsoleAsync();
            }

            await consoleHolder(endpoint.Instance);
            await endpoint.StopAsync();
        }

        private static void EnsureEndpoint(Endpoint endpoint)
        {
            if (endpoint == null) throw new Exception("IncorrectEndpoint");
        }
    }
}
