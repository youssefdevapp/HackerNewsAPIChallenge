using HackerNewsAPI.Clients.Interfaces;
using HackerNewsAPI.Entities;
using HackerNewsAPI.Repositories.Interfaces;

namespace HackerNewsAPI.Repositories
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly IHackerNewsClient _hackerNewsClient;

        public HackerNewsRepository(IHackerNewsClient hackerNewsClient)
            => _hackerNewsClient = hackerNewsClient;

        public async Task<List<long>> GetBestStoriesIdsAsync()
        {
            return await _hackerNewsClient.GetBestStoriesIdsAsync();
        }

        public async Task<StoryEntity> GetStoryAsync(long id)
        {
            return await _hackerNewsClient.GetStoryAsync(id);
        }
    }
}