using CleanArch.Application.Common.Interfaces.Repositories;
using CleanArch.Application.Common.Interfaces.Repositories.Command.Base;
using CleanArch.Application.Common.Interfaces.Repositories.Query.Base;
using CleanArch.Infrastructure.Configs;
using CleanArch.Infrastructure.Persistence;
using CleanArch.Infrastructure.Repository;
using CleanArch.Infrastructure.Repository.Command.Base;
using CleanArch.Infrastructure.Repository.Query.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CleanArch.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigurationSettings>(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var opt = serviceProvider.GetRequiredService<IOptions<ConfigurationSettings>>().Value;

            // For SQLServer Connection
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    opt.ConnectionStrings.ConfigurationDbConnection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                    });
            });

            services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<Func<ApplicationDbContext>>((provider) => provider.GetService<ApplicationDbContext>);
            services.AddScoped<DbFactory>();


            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}
