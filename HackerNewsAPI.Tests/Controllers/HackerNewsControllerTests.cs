using AutoFixture;
using HackerNewsAPI.Controllers;
using HackerNewsAPI.PublicContracts;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsAPI.Tests.Controllers
{
    [TestClass]
    public class HackerNewsControllerTests
    {
        private MockRepository mockRepository;
        private IFixture fixture;

        private Mock<IHackerNewsService> mockHackerNewsService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            fixture = new Fixture();

            mockHackerNewsService = mockRepository.Create<IHackerNewsService>();
        }

        private HackerNewsController CreateHackerNewsController()
        {
            return new HackerNewsController(mockHackerNewsService.Object);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(100)]
        public async Task GetBestStories_MaxStories_ReturnExpected(int maxOfStories)
        {
            // Arrange
            var hackerNewsController = CreateHackerNewsController();
            var stories = fixture.CreateMany<StoryResponse>(maxOfStories);

            mockHackerNewsService.Setup(s => s.GetBestStoriesAsync(maxOfStories)).ReturnsAsync(stories);

            // Act
            var result = await hackerNewsController.GetBestStories(maxOfStories);

            // Assert
            Assert.AreEqual(maxOfStories, result.Count());
            mockRepository.VerifyAll();
        }
    }
}