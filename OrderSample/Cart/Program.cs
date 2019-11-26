using System;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Cart
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new EndpointConfiguration(endpointName: "Cart");
            config.UseSerialization<NewtonsoftSerializer>();

            // 为endpoint创建run的资源
            config.EnableInstallers();

            // callback 配置
            config.EnableCallbacks();
            // 确保endpoint有唯一实例Id
            config.MakeInstanceUniquelyAddressable("uniqueId");

            var transport = config.UseTransport<RabbitMQTransport>().UseConventionalRoutingTopology();
            transport.ConnectionString("host=127.0.0.1;username=admin;password=1qaz2wsx3edc4rfv;virtualhost=rabbitmq_vhost;");

            var endpoint = Endpoint.Start(config).Result;
            await SendOrder(endpoint);
        }

        static async Task SendOrder(IEndpointInstance endpoint)
        {
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                var order = new PlaceOrder
                {
                    Price = await GetPrice(endpoint, 3)
                };

                await endpoint.Send("Order", order);
            }
        }

        private static async Task<int> GetPrice(IEndpointInstance endpoint, int weight)
        {
            var options = new SendOptions();
            options.SetDestination("Calculator");

            // Message Identity
            options.SetMessageId("Fred test");

            // Cancel request
            var cancelToken = new CancellationTokenSource();
            cancelToken.CancelAfter(TimeSpan.FromSeconds(5));

            var request = new PriceRequest { Weight = weight };

            // 不要在Handle里调用callback api，会导致死锁
            var response = await endpoint
                .Request<PriceResponse>(request, options, cancelToken.Token);
            return response.Price;
        }
    }
}
