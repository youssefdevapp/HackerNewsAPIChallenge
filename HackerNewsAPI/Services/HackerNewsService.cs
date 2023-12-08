using HackerNewsAPI.Entities;
using HackerNewsAPI.Filters;
using HackerNewsAPI.PublicContracts;
using HackerNewsAPI.Repositories.Interfaces;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<HackerNewsService> _logger;
        private readonly IHackerNewsRepository _hackerNewsRepository;
        private readonly IStoryMapperService _storyMapperService;

        public HackerNewsService(IMemoryCache cache, ILogger<HackerNewsService> logger, IHackerNewsRepository hackerNewsRepository, IStoryMapperService storyMapperService)
        {
            _cache = cache;
            _logger = logger;
            _hackerNewsRepository = hackerNewsRepository;
            _storyMapperService = storyMapperService;
        }

        public async Task<List<long>> GetBestStoriesIdsAsync()
        {
            try
            {
                List<long> getBestStoriesCache = _cache.Get<List<long>>(CacheKeys.CacheKeyBestStoriesIds);

                if (getBestStoriesCache != null)
                {
                    return getBestStoriesCache;
                }

                var bestStoriesIds = await _hackerNewsRepository.GetBestStoriesIdsAsync();

                if (bestStoriesIds != null)
                {
                    _cache.Set(CacheKeys.CacheKeyBestStoriesIds, bestStoriesIds, TimeSpan.FromMinutes(60));
                }
                else
                {
                    _cache.Remove(CacheKeys.CacheKeyBestStoriesIds);
                }

                return bestStoriesIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ids stories");
                throw new HttpResponseException(500, "An error occurred while fetching ids stories");
            }
        }

        public async Task<StoryResponse> GetStoryAsync(long id)
        {
            try
            {
                var result = new StoryResponse();

                var cacheId = $"{CacheKeys.CacheKeyStory}_{id}";
                StoryResponse GetStoryCache = _cache.Get<StoryResponse>(cacheId);

                if (GetStoryCache != null)
                {
                    return GetStoryCache;
                }

                var story = await _hackerNewsRepository.GetStoryAsync(id);
                if (story != null)
                {
                    result = _storyMapperService.StoryEntityToStoryResponse(story);
                    _cache.Set(cacheId, result, TimeSpan.FromHours(12));
                }
                else
                {
                    _cache.Remove(cacheId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching story");
                throw new HttpResponseException(500, "An error occurred while fetching story");
            }
        }

        public async Task<IEnumerable<StoryResponse>> GetBestStoriesAsync(int maxOfStories)
        {
            try
            {
                List<StoryResponse> storiesCache = _cache.Get<List<StoryResponse>>(CacheKeys.CacheKeyStories);

                if (storiesCache != null)
                {
                    return storiesCache.OrderByDescending(o => o.Score).Take(maxOfStories);
                }

                var ids = await GetBestStoriesIdsAsync();
                List<StoryResponse> results = new List<StoryResponse>();

                await Parallel.ForEachAsync(ids, async (id, cancellationToken) =>
                {
                    var story = await GetStoryAsync(id);
                    if (story != default(StoryResponse))
                    {
                        results.Add(story);
                    }
                });

                if (results.Any())
                {
                    _cache.Set(CacheKeys.CacheKeyStories, results, TimeSpan.FromHours(24));
                }
                else
                {
                    _cache.Remove(CacheKeys.CacheKeyStories);
                }

                return results.OrderByDescending(o => o.Score).Take(maxOfStories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stories");
                throw new HttpResponseException(500, "An error occurred while fetching stories");
            }
        }
    }
}