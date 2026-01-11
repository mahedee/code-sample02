using Microsoft.EntityFrameworkCore;
using Catalog.API.Data;
using Catalog.API.Services;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database
            builder.Services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // RabbitMQ Configuration
            builder.Services.AddSingleton<IConnectionProvider>(
                new ConnectionProvider("amqp://guest:guest@localhost:5672"));

            // Publisher for Inventory events
            builder.Services.AddSingleton<IPublisher>(provider =>
                new Publisher(
                    provider.GetService<IConnectionProvider>(),
                    "inventory.exchange",
                    ExchangeType.Topic));

            // Subscriber for Order events
            builder.Services.AddSingleton<ISubscriber>(provider =>
                new Subscriber(
                    provider.GetService<IConnectionProvider>(),
                    "order.exchange",
                    "order.created.queue",
                    "order.created",
                    ExchangeType.Topic));

            // Subscriber for Inventory Release events
            builder.Services.AddKeyedSingleton<ISubscriber>("ReleaseSubscriber", (provider, key) =>
                new Subscriber(
                    provider.GetService<IConnectionProvider>(),
                    "order.exchange",
                    "inventory.release.queue",
                    "inventory.release",
                    ExchangeType.Topic));

            // Subscriber for Payment events
            builder.Services.AddKeyedSingleton<ISubscriber>("PaymentSubscriber", (provider, key) =>
                new Subscriber(
                    provider.GetService<IConnectionProvider>(),
                    "payment.exchange",
                    "payment.confirmation.queue",
                    "payment.processed",
                    ExchangeType.Topic));

            // Background Services
            builder.Services.AddHostedService<OrderCreatedListener>();
            builder.Services.AddHostedService<InventoryReleaseListener>();
            builder.Services.AddHostedService<PaymentConfirmationListener>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Ensure database is created and seeded
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                context.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}
