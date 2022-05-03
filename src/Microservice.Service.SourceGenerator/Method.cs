using System.Collections.Generic;

namespace Microservice.Service.SourceGenerator
{
    public class Method
    {
        public string Name { get; set; }

        public string ReturnType { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public string HttpMethod { get; set; }
    }
}