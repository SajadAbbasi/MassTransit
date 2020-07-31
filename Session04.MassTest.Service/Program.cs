using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Session04.MassTest.Components.Consumers;
using Session04.MassTest.Contracts;
using System;
using System.Threading.Tasks;

namespace Session04.MassTest.Service
{


    public class Program
    {
        static async Task Main()
        {
            IHostBuilder builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddConsumersFromNamespaceContaining<OrderRegisteredHandler>();
                        cfg.AddBus(ConfigureBus);
                    });

                    services.AddHostedService<HostedService>();
                })
                .ConfigureLogging((hostingContext,logging)=>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });
            await builder.RunConsoleAsync();
        }

        static IBusControl ConfigureBus(IBusRegistrationContext provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ConfigureEndpoints(provider);
            });
        }
    }
}
