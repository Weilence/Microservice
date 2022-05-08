using System.Collections.Generic;

namespace Microservice.Consul
{
    public class ConsulConfig
    {
        public string Server { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public List<string> Tags { get; set; }
    }
}