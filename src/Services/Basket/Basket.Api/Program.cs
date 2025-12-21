
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(assembly);    
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Adding ValidationBehavior to MediatR pipeline
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Adding LoggingBehavior to MediatR pipeline
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("BasketDb")!);
    options.AutoCreateSchemaObjects = JasperFx.AutoCreate.All;
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
})
.UseLightweightSessions()
.ApplyAllDatabaseChangesOnStartup();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    //options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddHealthChecks()
.AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
.AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(options =>
{
    
});

app.UseHealthChecks("/health", 
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
