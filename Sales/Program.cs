using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sales
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Sales";
            await Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Sales");

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.ConnectionString("host=hostos;username=rabbitmq;password=rabbitmq");
                    transport.UseConventionalRoutingTopology();

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    endpointConfiguration.EnableInstallers();

                    var recoverability = endpointConfiguration.Recoverability();
                    recoverability.Immediate(settings => settings.NumberOfRetries(0));
                    recoverability.Delayed(settings => settings.NumberOfRetries(0));

                    return endpointConfiguration;
                })
                .RunConsoleAsync();
        }
    }
}
