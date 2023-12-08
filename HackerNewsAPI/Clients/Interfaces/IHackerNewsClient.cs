using HackerNewsAPI.Entities;

namespace HackerNewsAPI.Clients.Interfaces
{
    public interface IHackerNewsClient
    {
        Task<List<long>> GetBestStoriesIdsAsync();

        Task<StoryEntity> GetStoryAsync(long id);

        Task<UpdateItemsEntity> GetUpdateItemsAsync();
    }
}