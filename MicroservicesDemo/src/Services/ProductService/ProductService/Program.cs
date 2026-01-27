using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Services;
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
        Title = "Product Service API",
        Version = "v1",
        Description = "API for managing products in the microservices demo",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Product Service Team",
            Email = "products@microservicesdemo.com"
        }
    });
});

// Add Entity Framework
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add EventBus
builder.Services.AddSingleton<IEventBus, KafkaEventBus>();

// Add Event Handlers
builder.Services.AddScoped<OrderEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Product Service API Documentation";
    });
}

app.UseAuthorization();
app.MapControllers();

// Ensure database is created with retry logic
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var retryCount = 0;
    var maxRetries = 30;
    
    while (retryCount < maxRetries)
    {
        try
        {
            logger.LogInformation("Attempting to connect to database, attempt {Attempt}", retryCount + 1);
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
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    var orderEventHandler = scope.ServiceProvider.GetRequiredService<OrderEventHandler>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("Subscribing to OrderCreatedEvent...");
    await eventBus.SubscribeAsync<OrderCreatedEvent>(orderEventHandler.Handle);
    logger.LogInformation("Successfully subscribed to OrderCreatedEvent");
}

app.Run();