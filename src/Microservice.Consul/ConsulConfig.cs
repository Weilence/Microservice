namespace Microservice.Consul
{
    public class ConsulConfig
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string[] Tags { get; set; }
    }
}