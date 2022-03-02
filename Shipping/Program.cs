using Microsoft.Extensions.Hosting;
using NServiceBus;
using RetailSystem.Conventions;

namespace Shipping
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Shipping";
            await Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Shipping");

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.ConnectionString("host=hostos;username=rabbitmq;password=rabbitmq");
                    transport.UseConventionalRoutingTopology();

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    endpointConfiguration.EnableInstallers();

                    endpointConfiguration.Pipeline.Register(new TimerBehavior(), "Provides timing for each message handler");

                    return endpointConfiguration;
                })
                .RunConsoleAsync();
        }
    }
}
