using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Add MediatR to the Pipe line
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
