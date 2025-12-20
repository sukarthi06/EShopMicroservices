using Marten.Schema;

namespace Catalog.Api.Data;

public class MartenSeedHostedService(
    IServiceScopeFactory scopeFactory,
    ILogger<MartenSeedHostedService> logger,
    IConfiguration configuration) : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<MartenSeedHostedService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_configuration.GetValue("SEED_DATA", false))
        {
            _logger.LogInformation("Marten seeding disabled");
            return;
        }

        _logger.LogInformation("Starting Marten data seeding");

        using var scope = _scopeFactory.CreateScope();

        var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();
        var seeders = scope.ServiceProvider.GetServices<IInitialData>();

        foreach (var seeder in seeders)
        {
            await seeder.Populate(store, cancellationToken);
        }

        _logger.LogInformation("Marten data seeding completed");
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
