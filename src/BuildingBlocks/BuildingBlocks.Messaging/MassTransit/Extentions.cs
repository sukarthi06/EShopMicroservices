using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extentions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                config.AddConsumers(assembly);

            config.UsingRabbitMq((context, configurator) =>
            {                
                // Check Aspire connection string first
                var connectionString = configuration.GetConnectionString("rabbitmq");//rabbitmq - The name should match the one set in Aspire

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    configurator.Host(new Uri(connectionString));
                    configurator.ConfigureEndpoints(context);
                    return;
                }

                // Fallback to local/dev configuration
                var host = configuration["MessageBroker:Host"];
                var username = configuration["MessageBroker:UserName"] ?? "guest";
                var password = configuration["MessageBroker:Password"] ?? "guest";

                if (string.IsNullOrWhiteSpace(host))
                    throw new InvalidOperationException("RabbitMQ host is not configured.");

                configurator.Host(new Uri(host), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
