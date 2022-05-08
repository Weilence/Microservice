using System;
using System.Linq;
using Consul;
using Microservice.Service;

namespace Microservice.Consul
{
    public class ConsulResolveUrl : IResolveUrl
    {
        private readonly IConsulClient _consulClient;

        public ConsulResolveUrl(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public string ResolveUrl(string server, string name, string path)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "http://" + server + path;
            }
            else
            {
                var services = _consulClient.Catalog.Service(name).Result.Response;

                var service = GetService(services);

                if (service == null)
                {
                    throw new Exception("No service found");
                }

                return $"http://{service.ServiceAddress}:{service.ServicePort}{path}";
            }
        }

        private CatalogService GetService(CatalogService[] services)
        {
            return GetMachineService(services) ?? GetNonMachineService(services);
        }

        private CatalogService GetNonMachineService(CatalogService[] services)
        {
            return services.FirstOrDefault(m => !IsMachineTag(m.ServiceTags));
        }

        private static CatalogService GetMachineService(CatalogService[] services)
        {
            var service =
                services.FirstOrDefault(m =>
                    m.ServiceTags.Contains($"Machine:{Environment.MachineName}"));
            return service;
        }

        private bool IsMachineTag(string[] tags)
        {
            return tags.Any(m => m.StartsWith("Machine:"));
        }
    }
}