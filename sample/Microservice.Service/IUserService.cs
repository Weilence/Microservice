using Microservice.Api;

namespace Microservice.Service
{
    [Api(Address = "http://localhost:5018/User")]
    public interface IUserService
    {
        [Http(Method = "GET")]
        string Login(string username, string password);

        [Http(Method = "POST")]
        string Add(Dictionary<string, string> dic);
    }
}