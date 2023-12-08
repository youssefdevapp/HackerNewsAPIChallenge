using HackerNewsAPI.Entities;
using HackerNewsAPI.PublicContracts;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{
    /// <summary>
    /// Hacker News actions
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }
        /// <summary>
        /// Listing best stories descending order by score
        /// </summary>
        /// <param name="maxOfStories">number of stories as returned</param>
        /// <returns></returns>
        [HttpGet("/best/stories")]
        public async Task<IEnumerable<StoryResponse>> GetBestStories(int maxOfStories)
        {
            return await _hackerNewsService.GetBestStoriesAsync(maxOfStories);
        }
    }
}