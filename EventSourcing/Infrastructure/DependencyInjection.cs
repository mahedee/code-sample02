using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Common;
using EventStore.Client;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            // Event store database connection
            var settings = EventStoreClientSettings
                .Create("esdb://127.0.0.1:2113?tls=false&keepAliveTimeout=10000&keepAliveInterval=10000");

            var client = new EventStoreClient(settings);
            services.AddSingleton(client);

            // Register DbContext for SQL Server

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                    });
            });

            // Add event repository in pipeline
            //services.AddScoped<ICatalogItemAggregateRepository_old, CatalogItemAggregateRepository_old>();
            services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();
            services.AddEventsRepository<CatalogItem, Guid>();

            return services;

        }

        //private static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services)
        //    where TA : class, IAggregateRoot<TK>
        //{
        //    return services.AddSingleton(typeof(IAggregateRepository<TA, TK>), typeof(AggregateRepository<TA, TK>));
        //}

        private static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services)
    where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton(typeof(IAggregateRepository<TA, TK>), typeof(AggregateRepository<TA, TK>));
        }
    }
}
