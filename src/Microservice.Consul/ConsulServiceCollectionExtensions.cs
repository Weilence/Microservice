using Consul;
using Microservice.Consul;
using Microservice.Service;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsulConfig>(configuration);
            services.AddSingleton<IConsulClient, ConsulClient>();
            services.AddSingleton<IResolveUrl, ConsulResolveUrl>();
            services.AddHostedService<ConsulAgent>();

            return services;
        }
    }
}