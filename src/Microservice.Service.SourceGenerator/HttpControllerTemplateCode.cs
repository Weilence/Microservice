using System.Collections.Generic;

namespace Microservice.Service.SourceGenerator
{
    public partial class HttpControllerTemplate
    {
        public string Assembly { get; set; }
        public string Namespace { get; set; }
        public string Route { get; set; } = "[controller]/[action]";
        public string Name { get; set; }

        public List<Method> Methods { get; set; }
    }
}