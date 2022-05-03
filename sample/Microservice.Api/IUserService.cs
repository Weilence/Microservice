using System;
using Microservice.Core;

namespace Microservice.Api
{
    [Host(Name = "User", Path = "/User")]
    [Http]
    public interface IUserService
    {
        [Http(Method = "GET")]
        string Login(string username, string password);

        [Http(Method = "POST")]
        void Add(TestJson json);
    }

    public class TestJson
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}