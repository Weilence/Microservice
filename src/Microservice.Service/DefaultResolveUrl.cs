namespace Microservice.Service
{
    public class DefaultResolveUrl : IResolveUrl
    {
        public string ResolveUrl(string server, string name, string path)
        {
            return "http://" + server + path;
        }
    }
}