namespace Ordering.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // API Services
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure API Services
            return app;
        }
    }
}
