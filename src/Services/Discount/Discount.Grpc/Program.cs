using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// Detect if running inside a container (Azure, AWS, Docker)
var isContainer = builder.Configuration["DOTNET_RUNNING_IN_CONTAINER"] == "true";

if (isContainer)
{
    // Azure Container Apps requires HTTP/2 (cleartext) on port 8080 for gRPC
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080, o =>
        {
            o.Protocols = HttpProtocols.Http2;
        });
    });
}

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcReflection();

builder.Services.AddDbContext<DiscountContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DiscountDb")));

// Health checks
builder.Services.AddGrpcHealthChecks();
builder.Services.AddHealthChecks()
    .AddCheck("Self", () => HealthCheckResult.Healthy("Service is running"));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseMigration();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<DiscountService>();

app.MapGrpcHealthChecksService();

app.Run();
