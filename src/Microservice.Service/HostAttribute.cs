using System;

namespace Microservice.Service
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class HostAttribute : Attribute
    {
        public const string AttributeName = "Host";

        public string Name { get; set; }

        public string Server { get; set; }

        public string Path { get; set; }
    }
}