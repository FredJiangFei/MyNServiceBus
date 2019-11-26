using System;
using System.Threading.Tasks;
using Shipping.Endpoints;
using Shipping.Helpers;

namespace Shipping
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var endpoint = new Endpoint();
            await EndpointHost.RunAsConsoleAsync(endpoint);
        }
    }
}
