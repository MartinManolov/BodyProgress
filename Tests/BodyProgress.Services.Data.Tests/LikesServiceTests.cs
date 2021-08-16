using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BodyProgress.Services.Data.Tests
{
    public class LikesServiceTests
    {
        [Fact]
        public async Task AddLikeTest()
        {
            var testData = new List<Like>();
            var repo = new Mock<IDeletableEntityRepository<Like>>();
            repo.Setup(r => r.AllWithDeleted())
                .Returns(testData.AsQueryable);
            repo.Setup(r => r.AddAsync(It.IsAny<Like>()))
                .Callback(() => testData.Add(It.IsAny<Like>()));
            var service = new LikesService(repo.Object);

            var likeId = await service.Add("testUserId", "testPostId");

            Assert.Single(testData);
        }

        [Fact]
        public async Task DeleteLikeTest()
        {
            var testData = new List<Like>
            {
                new Like
                {
                    OwnerId = "testUser1",
                    PostId = "testPostId1",
                },
                new Like
                {
                    OwnerId = "testUser2",
                    PostId = "testPostId2",
                },
            };
            var repo = new Mock<IDeletableEntityRepository<Like>>();
            repo.Setup(r => r.All())
                .Returns(testData.AsQueryable);
            repo.Setup(r => r.Delete(It.IsAny<Like>()))
                .Callback(() => testData.Remove(It.IsAny<Like>()));
            var service = new LikesService(repo.Object);

            var isDeletedTrue = await service.Delete("testUser1", "testPostId1");
            var isdeletedFalse = await service.Delete("testUser1", "testPostId2");

            Assert.True(isDeletedTrue);
            Assert.False(isdeletedFalse);
        }

        [Fact]
        public void GetLikesByPostIdTest()
        {
            var testData = new List<Like>
            {
                new Like
                {
                    OwnerId = "testUser1",
                    PostId = "testPostId1",
                },
                new Like
                {
                    OwnerId = "testUser2",
                    PostId = "testPostId2",
                },
                new Like
                {
                    OwnerId = "testUser1",
                    PostId = "testPostId2",
                },
            };
            var repo = new Mock<IDeletableEntityRepository<Like>>();
            repo.Setup(r => r.AllAsNoTracking())
                .Returns(testData.AsQueryable);
            var service = new LikesService(repo.Object);

            var count = service.GetByPostId("testPostId2");

            Assert.Equal(2, count);
        }
    }
}
