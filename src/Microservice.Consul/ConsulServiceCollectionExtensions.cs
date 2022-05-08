using System;
using Consul;
using Microservice.Consul;
using Microservice.Service;
using Microsoft.AspNetCore.Builder;
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

                return new ConsulClient(config =>
                {
                    if (!string.IsNullOrEmpty(consulConfig.Server))
                    {
                        config.Address = new Uri(consulConfig.Server);
                    }

                    if (!string.IsNullOrEmpty(consulConfig.Token))
                    {
                        config.Token = consulConfig.Token;
                    }
                });
            });
            services.AddSingleton<IResolveUrl, ConsulResolveUrl>();
            services.AddHostedService<ConsulAgent>();

            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/consul/health")
                {
                    context.Response.StatusCode = 200;
                    return;
                }

                await next();
            });


            return app;
        }
    }
}