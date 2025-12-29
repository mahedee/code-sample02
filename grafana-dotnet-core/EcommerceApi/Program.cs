using EcommerceApi.Data;
using EcommerceApi.Repositories;
using Microsoft.EntityFrameworkCore;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Configure metrics with Prometheus formatters and web tracking
// This enables comprehensive HTTP request monitoring and Prometheus integration for Grafana
builder.Host.UseMetricsWebTracking().UseMetrics(options =>
{
    options.EndpointOptions = endpointoptions =>
    {
        // Configure Prometheus text formatter for /metrics-text endpoint (human readable)
        endpointoptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        // Configure Prometheus protobuf formatter for /metrics endpoint (efficient binary format)
        endpointoptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        // Disable environment info endpoint to reduce security exposure
        endpointoptions.EnvironmentInfoEndpointEnabled = false;
    };
}).UseMetricsWebTracking(options =>
{
    // Enable Apdex (Application Performance Index) tracking for user satisfaction monitoring
    // Apdex measures response time satisfaction: Satisfied/Tolerating/Frustrated requests
    options.ApdexTrackingEnabled = true;
    // Set Apdex T threshold to 0.1 seconds (100ms) - responses faster than this are "satisfied"
    options.ApdexTSeconds = 0.1;
});

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Ecommerce API", 
        Version = "v1",
        Description = "A simple ecommerce API for product management"
    });
});

// Configure Entity Framework with In-Memory Database
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseInMemoryDatabase("EcommerceInMemoryDb"));

// Register Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Enable metrics collection middleware - automatically tracks HTTP requests/responses
// This captures timing, status codes, and other HTTP metrics for Grafana monitoring
app.UseMetricsAllMiddleware();
// Expose metrics endpoints (/metrics, /metrics-text, /ping) for Prometheus scraping
app.UseMetricsAllEndpoints();

// Map controllers
app.MapControllers();

app.Run();
