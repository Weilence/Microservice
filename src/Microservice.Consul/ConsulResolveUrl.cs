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
                var queryResult = _consulClient.Catalog.Service(name).Result;
                var service = queryResult.Response[0];
                var address = "http://" + service.ServiceAddress + ":" + service.ServicePort + path;
                return address;
            }
        }
    }
}