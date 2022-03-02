using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Billing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Billing";
            await Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Billing");

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.ConnectionString("host=hostos;username=rabbitmq;password=rabbitmq");
                    transport.UseConventionalRoutingTopology();

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .RunConsoleAsync();
        }
    }
}
