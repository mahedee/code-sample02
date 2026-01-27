using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Services;
using EventBus;
using EventBus.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Order Service API",
        Version = "v1",
        Description = "API for managing orders in the microservices demo",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Order Service Team",
            Email = "orders@microservicesdemo.com"
        }
    });
});

// Add Entity Framework
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add EventBus and handlers
builder.Services.AddSingleton<IEventBus, KafkaEventBus>();
builder.Services.AddSingleton<ProductEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Order Service API Documentation";
    });
}

app.UseAuthorization();
app.MapControllers();

// Ensure database is created with retry logic
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var retryCount = 0;
    var maxRetries = 30;
    
    while (retryCount < maxRetries)
    {
        try
        {
            logger.LogInformation("Attempting to connect to database, attempt {Attempt}", retryCount + 1);
            
            // For demo purposes, we'll recreate the database to ensure schema is current
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            logger.LogInformation("Database connection successful!");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            logger.LogWarning("Database connection failed (attempt {Attempt}/{MaxAttempts}): {Error}", retryCount, maxRetries, ex.Message);
            
            if (retryCount >= maxRetries)
            {
                logger.LogError("Failed to connect to database after {MaxAttempts} attempts. Exiting.", maxRetries);
                throw;
            }
            
            await Task.Delay(2000);
        }
    }
}

// Subscribe to events
try
{
    var eventBus = app.Services.GetRequiredService<IEventBus>();
    var productEventHandler = app.Services.GetRequiredService<ProductEventHandler>();
    
    app.Logger.LogInformation("Setting up event subscriptions...");
    
    await eventBus.SubscribeAsync<ProductCreatedEvent>(productEventHandler.Handle);
    app.Logger.LogInformation("Subscribed to ProductCreatedEvent");
    
    await eventBus.SubscribeAsync<ProductUpdatedEvent>(productEventHandler.Handle);
    app.Logger.LogInformation("Subscribed to ProductUpdatedEvent");
    
    app.Logger.LogInformation("Event subscriptions setup completed");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to setup event subscriptions");
}

app.Run();