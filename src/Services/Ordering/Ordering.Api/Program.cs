using Ordering.Api;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// Add services to the container.

builder.Services
    .AddApplicationService(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddMediator(options => 
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors =
    [
        typeof(eShop.BuildingBlocks.Behaviors.ValidationBehavior<,>),
        typeof(eShop.BuildingBlocks.Behaviors.LoggingBehavior<,>),
        typeof(Ordering.Infrastructure.Data.Behaviors.DomainEventDispatchBehavior<,>)
    ];
});

var app = builder.Build();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseApiServices();

await app.InitialiseDatabaseAsync();

app.Run();
