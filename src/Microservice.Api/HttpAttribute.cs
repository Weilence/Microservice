using System;

namespace Microservice.Api
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpAttribute : Attribute
    {
        public const string AttributeName = "Http";

        public string Method { get; set; }
        public string Path { get; set; }
    }
}