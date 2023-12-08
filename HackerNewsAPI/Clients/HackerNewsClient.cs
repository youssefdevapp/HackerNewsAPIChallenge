using HackerNewsAPI.Clients.Interfaces;
using HackerNewsAPI.Entities;
using System.Text.Json;

namespace HackerNewsAPI.Clients
{
    public class HackerNewsClient : IHackerNewsClient
    {
        private readonly HttpClient _httpClient;

        public HackerNewsClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<List<long>> GetBestStoriesIdsAsync()
        {
            var response = await _httpClient.GetAsync("/v0/beststories.json");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<long>>();
        }

        public async Task<StoryEntity> GetStoryAsync(long id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await _httpClient.GetAsync($"/v0/item/{id}.json");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StoryEntity>(options);
        }

        public async Task<UpdateItemsEntity> GetUpdateItemsAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await _httpClient.GetAsync("/v0/updates.json");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateItemsEntity>(options);
        }
    }
}