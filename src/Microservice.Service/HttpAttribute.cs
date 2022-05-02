using System;

namespace Microservice.Service
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class HttpAttribute : Attribute
    {
        public const string AttributeName = "Http";

        public string Method { get; set; }
        public string Template { get; set; }
    }
}