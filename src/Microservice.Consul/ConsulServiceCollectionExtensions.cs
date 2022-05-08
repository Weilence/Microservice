using System;
using Consul;
using Microservice.Consul;
using Microservice.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsulConfig>(configuration);
            services.AddSingleton<IConsulClient>(provider =>
            {
                var consulConfig = provider.GetRequiredService<IOptions<ConsulConfig>>().Value;
                var server = consulConfig.Server;

                return new ConsulClient(config =>
                {
                    if (!string.IsNullOrEmpty(server))
                    {
                        config.Address = new Uri(server);
                    }
                });
            });
            services.AddSingleton<IResolveUrl, ConsulResolveUrl>();
            services.AddHostedService<ConsulAgent>();

            return services;
        }
    }
}