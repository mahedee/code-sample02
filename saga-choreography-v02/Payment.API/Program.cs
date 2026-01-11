using Microsoft.EntityFrameworkCore;
using Payment.API.Data;
using Payment.API.Services;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace Payment.API
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
            builder.Services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Payment Service
            builder.Services.AddScoped<IPaymentProcessor, PaymentProcessor>();

            // RabbitMQ Configuration
            builder.Services.AddSingleton<IConnectionProvider>(
                new ConnectionProvider("amqp://guest:guest@localhost:5672"));

            // Publisher for Payment events
            builder.Services.AddSingleton<IPublisher>(provider =>
                new Publisher(
                    provider.GetService<IConnectionProvider>(),
                    "payment.exchange",
                    ExchangeType.Topic));

            // Subscriber for Inventory events
            builder.Services.AddSingleton<ISubscriber>(provider =>
                new Subscriber(
                    provider.GetService<IConnectionProvider>(),
                    "inventory.exchange",
                    "inventory.reserved.queue",
                    "inventory.reserved",
                    ExchangeType.Topic));

            // Background Services
            builder.Services.AddHostedService<InventoryReservedListener>();

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
                var context = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
                context.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}