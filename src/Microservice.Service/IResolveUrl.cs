namespace Microservice.Service
{
    public interface IResolveUrl
    {
        string ResolveUrl(string server, string name, string path);
    }
}