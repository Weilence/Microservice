using System;

namespace Microservice.Core
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class HttpAttribute: Attribute
    {
        public const string AttributeName = "Http";

        public string Method { get; set; }
        public string Route { get; set; }
    }
}