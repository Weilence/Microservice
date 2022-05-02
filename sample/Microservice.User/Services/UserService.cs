using Microservice.Api;

namespace Microservice.User.Services;

public class UserService : IUserService
{
    public string Login(string username, string password)
    {
        if (username == "admin" && password == "123456")
        {
            return username + ":" + password;
        }

        return "error";
    }

    public string Add(Dictionary<string, string> dic)
    {
        throw new NotImplementedException();
    }
}