using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//if (Environment.GetEnvironmentVariable("ASPIRE_ENABLED") == "true")
//    builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "http://localhost:8080",
                            "https://eshop-ui.wittydune-62951da6.eastus.azurecontainerapps.io") // Angular app
            .AllowAnyHeader()
            .AllowAnyMethod();
            //.WithExposedHeaders("*");
        //.AllowCredentials(); // only if using cookies/auth
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

//if (Environment.GetEnvironmentVariable("ASPIRE_ENABLED") == "true")
//    app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

//app.Use(async (context, next) =>
//{
//    if (context.Request.Method == HttpMethods.Options)
//    {
//        context.Response.StatusCode = StatusCodes.Status204NoContent;
//        return;
//    }

//    await next();
//});

app.UseCors("CorsPolicy");

app.UseRouting();


app.UseRateLimiter();

app.MapReverseProxy();

app.Run();
