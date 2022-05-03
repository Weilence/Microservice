using System.Collections.Generic;

namespace Microservice.Service.SourceGenerator
{
    public partial class AddMicroserviceTemplate
    {
        public List<ClassInfo> ClassInfos { get; set; }
    }

    public class ClassInfo
    {
        public string Interface { get; set; }
        
        public string Class { get; set; }
    }
}