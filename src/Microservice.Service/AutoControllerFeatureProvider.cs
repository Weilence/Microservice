using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microservice.Service
{
    public class AutoControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var candidates = currentAssembly.GetExportedTypes()
                .Where(m => m.GetInterfaces().Any(n => n.GetCustomAttributes<HttpAttribute>().Any()));

            foreach (var candidate in candidates)
            {
                feature.Controllers.Add(candidate.GetTypeInfo());
            }
        }
    }
}