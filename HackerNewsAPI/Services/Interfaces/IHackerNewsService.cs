using HackerNewsAPI.Entities;
using HackerNewsAPI.PublicContracts;

namespace HackerNewsAPI.Services.Interfaces
{
    public interface IHackerNewsService
    {
        Task<List<long>> GetBestStoriesIdsAsync();

        Task<StoryResponse> GetStoryAsync(long id);

        Task<IEnumerable<StoryResponse>> GetBestStoriesAsync(int maxOfStories);
    }
}