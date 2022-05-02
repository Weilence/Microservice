using System;

namespace Microservice.Api
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ApiAttribute : Attribute
    {
        public const string AttributeName = "Api";

        public string Name { get; set; }

        public string Address { get; set; }

        public string Path { get; set; } = "[controller]/[action]";
    }
}