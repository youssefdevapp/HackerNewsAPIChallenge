using AutoFixture;
using HackerNewsAPI.Entities;
using HackerNewsAPI.PublicContracts;
using HackerNewsAPI.Repositories.Interfaces;
using HackerNewsAPI.Services;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsAPI.Tests.Services
{
    [TestClass]
    public class HackerNewsServiceTests
    {
        private MockRepository mockRepository;
        private IFixture fixture;

        private IMemoryCache _memoryCache;
        private Mock<ILogger<HackerNewsService>> mockLogger;
        private Mock<IHackerNewsRepository> mockHackerNewsRepository;
        private Mock<IStoryMapperService> mockStoryMapperService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            fixture = new Fixture();

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            _memoryCache = memoryCache;
            mockLogger = new Mock<ILogger<HackerNewsService>>();
            mockHackerNewsRepository = mockRepository.Create<IHackerNewsRepository>();
            mockStoryMapperService = mockRepository.Create<IStoryMapperService>();
        }

        private HackerNewsService CreateService()
        {
            return new HackerNewsService(
                _memoryCache,
                mockLogger.Object,
                mockHackerNewsRepository.Object,
                mockStoryMapperService.Object);
        }

        [TestMethod]
        public async Task GetBestStoriesIdsAsync_Count_NumberExpected()
        {
            // Arrange
            var service = CreateService();
            var storiesIds = fixture.CreateMany<long>(100);
            mockHackerNewsRepository.Setup(s => s.GetBestStoriesIdsAsync()).ReturnsAsync(storiesIds.ToList());

            // Act
            var result = await service.GetBestStoriesIdsAsync();

            // Assert
            Assert.AreEqual(100, result.Count());
            mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetStoryAsync_ById_ReturnCorrectId(long id)
        {
            // Arrange
            var service = CreateService();

            var storyEntity = fixture.Build<StoryEntity>().With(n => n.Id, id).Create();
            var storyResponse = fixture.Build<StoryResponse>().With(n => n.Id, id).Create();

            mockHackerNewsRepository
                .Setup(s => s.GetStoryAsync(id))
                .ReturnsAsync(storyEntity);

            mockStoryMapperService
                .Setup(s => s.StoryEntityToStoryResponse(storyEntity))
                .Returns(storyResponse);

            // Act
            var result = await service.GetStoryAsync(id);

            // Assert
            Assert.AreEqual(id, result.Id);
            mockRepository.VerifyAll();
        }

        [TestMethod]
        [DataRow(5)]
        [DataRow(10)]
        public async Task GetBestStoriesAsync_FromRepository_Succesfully(int maxOfStories)
        {
            // Arrange
            var service = CreateService();
            var storiesIds = fixture.CreateMany<long>(100);

            mockHackerNewsRepository.Setup(s => s.GetBestStoriesIdsAsync()).ReturnsAsync(storiesIds.ToList());

            foreach (var id in storiesIds)
            {
                var storyResponse = fixture.Create<StoryResponse>();
                var storyEntity = fixture.Build<StoryEntity>().With(n => n.Id, id).Create();

                mockHackerNewsRepository
                    .Setup(s => s.GetStoryAsync(id))
                    .ReturnsAsync(storyEntity);

                mockStoryMapperService
                    .Setup(s => s.StoryEntityToStoryResponse(storyEntity))
                    .Returns(storyResponse);
            }

            // Act
            var result = await service.GetBestStoriesAsync(maxOfStories);

            // Assert
            Assert.AreEqual(maxOfStories, result.Count());
            mockRepository.VerifyAll();
        }
    }
}