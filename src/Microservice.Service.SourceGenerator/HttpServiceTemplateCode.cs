using System.Collections.Generic;

namespace Microservice.Service.SourceGenerator
{
    public partial class HttpServiceTemplate
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string Server { get; set; }
        public List<Method> Methods { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
    }
}