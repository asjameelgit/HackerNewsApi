using HackerNewsApi.Interface;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HackerNewsApi.Service
{
    public class GenericApiService : IGenericApiService
    {
        private readonly HttpClient _httpClient;

        public GenericApiService(IHttpClientFactory httpClientFactory) 
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<T> GetAsync<T>(Uri requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();           
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
