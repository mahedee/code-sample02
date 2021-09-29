using Catalog.API.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace Catalog.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
            });

            // Configure Sqlite
            services.AddDbContext<CatalogContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
            services.AddSingleton<IPublisher>(p => new Publisher(p.GetService<IConnectionProvider>(),
                "catalog_exchange",
                ExchangeType.Topic));

            services.AddSingleton<ISubscriber>(s => new Subscriber(s.GetService<IConnectionProvider>(),
                "order_exchange",
                "order_response_queue",
                "order_created_routingkey",
                ExchangeType.Topic
                ));

            services.AddHostedService<OrderCreatedListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
