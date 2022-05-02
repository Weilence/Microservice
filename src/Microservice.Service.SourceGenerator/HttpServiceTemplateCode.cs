using System.Collections.Generic;

namespace Microservice.Service.SourceGenerator
{
    public partial class HttpServiceTemplate
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string Address { get; set; }
        public List<Method> Methods { get; set; }
    }
}