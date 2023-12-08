using HackerNewsAPI.Entities;

namespace HackerNewsAPI.Repositories.Interfaces
{
    public interface IHackerNewsRepository
    {
        Task<List<long>> GetBestStoriesIdsAsync();

        Task<StoryEntity> GetStoryAsync(long id);
    }
}