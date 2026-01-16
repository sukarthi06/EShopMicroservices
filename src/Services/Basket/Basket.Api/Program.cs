using Basket.Api.Basket.StoreBasket;
using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using eShop.BuildingBlocks.Exceptions.Handler;
//using eShop.BuildingBlocks.CQRS;
using HealthChecks.UI.Client;
using Mediator;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// Add services to the container.
// Application services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
//builder.Services.AddMediator(cfg => {
//    cfg.RegisterServicesFromAssembly(assembly);    
//    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Adding ValidationBehavior to MediatR pipeline
//    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Adding LoggingBehavior to MediatR pipeline
//});

//var assemblies = new[]
//{
//    typeof(StoreBasketCommand).Assembly,          // Command definitions
//    typeof(StoreBasketCommandHandler).Assembly,   // Handlers
//    typeof(GetBasketQueryHandler).Assembly
//    //typeof(ValidationBehavior<,>).Assembly       // Pipeline behaviors
//};
//builder.Services.AddCQRS(
//    assemblies
//);
//builder.Services.AddMediator();

//builder.Services.AddMediator();
//builder.Services.AddCQRS(assembly);
//builder.Services.AddMediator();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors =
    [
        typeof(eShop.BuildingBlocks.Behaviors.ValidationBehavior<,>),
        typeof(eShop.BuildingBlocks.Behaviors.LoggingBehavior<,>)
    ];
});
builder.Services.AddValidatorsFromAssemblyContaining<StoreBasketCommandValidator>();


// Data services
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("BasketDb")!);
    options.AutoCreateSchemaObjects = JasperFx.AutoCreate.All;
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
})
.UseLightweightSessions()
.ApplyAllDatabaseChangesOnStartup();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    //options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Grpc services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
//.AddServiceDiscovery();
// only for development purposes otherwise use proper certificate
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

//Async messaging services
builder.Services.AddMessageBroker(builder.Configuration);

// Cross cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
.AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
.AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(options =>
{
    
});
//var mediator = app.Services.GetRequiredService<IMediator>();

app.UseHealthChecks("/health", 
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
