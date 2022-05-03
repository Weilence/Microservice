using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microservice.Service
{
    public static class MicroserviceExtensions
    {
        public static IServiceCollection AddMicroservice(this IServiceCollection services)
        {
            services.AddSingleton<HttpServiceClient>();
            services.TryAddSingleton<IResolveUrl, DefaultResolveUrl>();
            return services;
        }
    }
}