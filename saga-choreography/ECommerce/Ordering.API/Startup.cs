using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.API.Db;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using Shared;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });


            //services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));

            // Configure IPublisher
            //services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
            //    "order-exchange",
            //    ExchangeType.Topic));

            //services.AddMassTransit(config => {
            //    config.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        cfg.Host("amqp://guest:guest@localhost:5672");
            //    });
            //});

            // Configure Sqlite
            services.AddDbContext<OrderingContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));


            //Configure rabbitmq
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));

            services.AddSingleton<IPublisher>(p => new Publisher(p.GetService<IConnectionProvider>(),
                "order_exchange", // exchange name
                ExchangeType.Topic));

            services.AddSingleton<ISubscriber>(s => new Subscriber(s.GetService<IConnectionProvider>(),
                "catalog_exchange", // Exchange name
                "catalog_response_queue", //queue name
                "catalog_response_routingkey", // routing key
                ExchangeType.Topic));

            services.AddHostedService<CatalogResponseListener>();

            // Configure rabbitmq
            //services.AddMassTransit(x =>
            //{
            //    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
            //    {
            //        config.Host(new Uri(Settings.RabbitMqRootUri), h =>
            //        {
            //            h.Username(Settings.RabbitMqUserName);
            //            h.Password(Settings.RabbitMqPassword);
            //        });

            //    }));
            //});

            //Bus.Factory.CreateUsingInMemory(cfg =>
            //{
            //    cfg.UseJsonSerializer();
            //});


            //services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            //{
            //    config.Host("amqp://guest:guest@localhost:5672");
            //    config.ReceiveEndpoint("temp-queue", c =>
            //    {
            //        c.Handler<Order>(ctx =>
            //        {
            //            return Console.Out.WriteLineAsync(ctx.Message.Name);
            //        });
            //    });
            //});

            //bus.Start();
            //bus.Publish(new Order { Name = "Test Name" });
        }
    }
}
