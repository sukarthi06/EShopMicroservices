using HealthChecks.UI.Client;
using Marten.Schema;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. container part.
builder.AddServiceDefaults();

var assembly = typeof(Program).Assembly;


builder.Services.AddMediatR(config => 
    { 
        config.RegisterServicesFromAssembly(assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));//Adding ValidationBehavior to MediatR pipeline
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));//Adding LoggingBehavior to MediatR pipeline
    }
);
builder.Services.AddValidatorsFromAssembly(assembly);//FluentValidation

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

// Register your seeder(s)
builder.Services.AddSingleton<IInitialData, CatalogInitialData>();

// Hosted service that runs them
builder.Services.AddHostedService<MartenSeedHostedService>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogDb")!);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the http request pipeline.
app.MapCarter();

app.UseExceptionHandler(options =>
{
   // options.RunExceptionHandler<CustomExceptionHandler>();
});

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
