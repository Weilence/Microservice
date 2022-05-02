using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservice.Service
{
    public class HttpServiceClient
    {
        private readonly HttpClient _httpClient;

        public HttpServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(HttpServiceClient));
        }

        public async Task<T> Get<T>(string url)
        {
            return await _httpClient.GetFromJsonAsync<T>(url);
        }

        public async Task<T> Post<T>(string url, object obj)
        {
            var res = await _httpClient.PostAsJsonAsync(url, obj);
            return await res.Content.ReadFromJsonAsync<T>();
        }
    }
}