using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace Shipping.Endpoints
{
    public sealed class Endpoint
    {
        internal IEndpointInstance Instance { get; private set; }

        public async Task StartAsync()
        {
            var config = new EndpointConfiguration("Shipping");

            config.UseSerialization<NewtonsoftSerializer>();
            config.EnableInstallers();

            var transport = config.UseTransport<RabbitMQTransport>().UseConventionalRoutingTopology();
            transport.ConnectionString("host=127.0.0.1;username=admin;password=1qaz2wsx3edc4rfv;virtualhost=rabbitmq_vhost;");

            var persistence = config.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MySql>();
            persistence.ConnectionBuilder(() => new MySqlConnection("server=127.0.0.1;user=root;database=edi_data;port=3306;password=1qaz2wsx3edc4rfv;AllowUserVariables=True;AutoEnlist=false"));

            Instance = await NServiceBus.Endpoint.Start(config);
        }

        public async Task StopAsync()
        {
            if (Instance == null) return;
            await Instance.Stop();
            Instance = null;
        }
    }
}
