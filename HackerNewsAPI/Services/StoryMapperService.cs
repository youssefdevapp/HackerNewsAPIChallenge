using HackerNewsAPI.Entities;
using HackerNewsAPI.PublicContracts;
using HackerNewsAPI.Services.Interfaces;

namespace HackerNewsAPI.Services
{
    public class StoryMapperService : IStoryMapperService
    {
        public StoryResponse StoryEntityToStoryResponse(StoryEntity storeEntity)
        {
            if (storeEntity == null)
            {
                return default;
            }

            return new StoryResponse()
            {
                Id = storeEntity.Id,
                Title = storeEntity.Title,
                Uri = storeEntity.Url,
                PostedBy = storeEntity.By,
                Time = DateTimeOffset.FromUnixTimeMilliseconds(storeEntity.Time).DateTime,
                Score = storeEntity.Score,
                CommentCount = storeEntity.Kids != null ? storeEntity.Kids.Count() : 0
            };
        }
    }
}
