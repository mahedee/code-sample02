using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Services;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace Order.API
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
            builder.Services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // RabbitMQ Configuration
            builder.Services.AddSingleton<IConnectionProvider>(
                new ConnectionProvider("amqp://guest:guest@localhost:5672"));

            // Publisher for Order events
            builder.Services.AddSingleton<IPublisher>(provider =>
                new Publisher(
                    provider.GetService<IConnectionProvider>(),
                    "order.exchange",
                    ExchangeType.Topic));

            // Subscriber for Inventory events
            builder.Services.AddSingleton<ISubscriber>(provider =>
                new Subscriber(
                    provider.GetService<IConnectionProvider>(),
                    "inventory.exchange",
                    "inventory.response.queue",
                    "inventory.reserved",
                    ExchangeType.Topic));

            // Subscriber for Payment events
            builder.Services.AddKeyedSingleton<ISubscriber>("PaymentSubscriber", (provider, key) =>
                new Subscriber(
                    provider.GetService<IConnectionProvider>(),
                    "payment.exchange",
                    "payment.response.queue",
                    "payment.processed",
                    ExchangeType.Topic));

            // Background Services
            builder.Services.AddHostedService<InventoryResponseListener>();
            builder.Services.AddHostedService<PaymentResponseListener>();

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

            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                context.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}