using System.Collections.Generic;
using Microservice.Service;

namespace Microservice.Api
{
    [Host(Name = "User", Path = "/User")]
    [Http]
    public interface IUserService
    {
        [Http(Method = "GET")]
        string Login(string username, string password);

        [Http(Method = "POST")]
        void Add(Dictionary<string, string> dic);
    }
}