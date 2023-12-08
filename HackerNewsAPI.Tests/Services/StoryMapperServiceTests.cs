using AutoFixture;
using HackerNewsAPI.Entities;
using HackerNewsAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace HackerNewsAPI.Tests.Services
{
    [TestClass]
    public class StoryMapperServiceTests
    {
        private MockRepository mockRepository;
        private IFixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            fixture = new Fixture();
        }

        private StoryMapperService CreateService()
        {
            return new StoryMapperService();
        }

        [TestMethod]
        public void StoryEntityToStoryResponse_Mapped_Correctly()
        {
            // Arrange
            var service = CreateService();
            StoryEntity storeEntity = fixture.Create<StoryEntity>();

            // Act
            var result = service.StoryEntityToStoryResponse(storeEntity);

            // Assert
            Assert.AreEqual(storeEntity.Id, result.Id);
            Assert.AreEqual(storeEntity.Title, result.Title);
            Assert.AreEqual(storeEntity.Url, result.Uri);
            Assert.AreEqual(storeEntity.By, result.PostedBy);
            Assert.AreEqual(DateTimeOffset.FromUnixTimeMilliseconds(storeEntity.Time).DateTime, result.Time);
            Assert.AreEqual(storeEntity.Score, result.Score);
            Assert.AreEqual(storeEntity.Kids.Count(), result.CommentCount);

            mockRepository.VerifyAll();
        }
    }
}