using HackerNewsAPI.Entities;
using HackerNewsAPI.PublicContracts;

namespace HackerNewsAPI.Services.Interfaces
{
    public interface IStoryMapperService
    {
        StoryResponse StoryEntityToStoryResponse(StoryEntity storeEntity);
    }
}