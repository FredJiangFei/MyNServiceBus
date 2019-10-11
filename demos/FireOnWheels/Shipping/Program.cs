using System;
using System.Threading.Tasks;
using Shipping.Helpers;
using Endpoint = Shipping.Endpoints.Endpoint;

namespace Shipping
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "Shipping";
            var endpoint = new Endpoint();
            await EndpointHost.RunAsConsoleAsync(endpoint);
        }
    }
}
