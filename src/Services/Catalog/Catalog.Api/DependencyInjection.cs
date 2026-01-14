using System.Reflection;

namespace Catalog.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCQRS(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            // Validators
            services.AddValidatorsFromAssemblies(assemblies);

            // Command handlers
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Query handlers
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Command decorators
            services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator<,>));
            services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator<,>));

            // Query decorators
            services.Decorate(typeof(IQueryHandler<,>), typeof(QueryValidationDecorator<,>));
            services.Decorate(typeof(IQueryHandler<,>), typeof(QueryLoggingDecorator<,>));

            // Executors
            services.AddScoped<ICommandExecutor, CommandExecutor>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();

            return services;
        }
    }
}
