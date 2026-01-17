using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("OrderDb");

            // Add services to the container.
            services.AddSingleton<AuditableEntityInterceptor>();
            //services.AddSingleton<DispatchDomainEventsInterceptor>();

            // Infrastructure Services
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>()//,
                //sp.GetRequiredService<DispatchDomainEventsInterceptor>()
                );

                options.UseNpgsql(connectionString,npgsqlOptionsAction =>
                {
                    npgsqlOptionsAction.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
                });
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}
