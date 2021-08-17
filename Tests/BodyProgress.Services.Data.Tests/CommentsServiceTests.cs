namespace BodyProgress.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Moq;
    using Xunit;

    public class CommentsServiceTests
    {
        [Fact]
        public async Task AddCommentTest()
        {
            var testData = new List<Comment>();
            var repo = new Mock<IDeletableEntityRepository<Comment>>();
            repo.Setup(r => r.AddAsync(It.IsAny<Comment>()))
                .Callback(() => testData.Add(It.IsAny<Comment>()));

            var comment = new CommentInputModel
            {
                PostId = "test1",
                TextContent = "text",
            };

            var service = new CommentsService(repo.Object);
            await service.Add(comment, "testId");

            Assert.Single(testData);
        }

        [Fact]
        public async Task DeleteCommentTest()
        {
            var testData = new List<Comment>
            {
                new Comment
                {
                    Id = "testId",
                    OwnerId = "test1",
                    TextContent = "test",
                    PostId = "test1postId",
                },
            };
            var repo = new Mock<IDeletableEntityRepository<Comment>>();
            repo.Setup(r => r.Delete(It.IsAny<Comment>()))
                .Callback(() => testData[0].IsDeleted = true);
            repo.Setup(r => r.All())
                .Returns(testData.AsQueryable);

            var service = new CommentsService(repo.Object);
            await service.Delete("testId", "test1");

            Assert.True(testData[0].IsDeleted);
        }

        [Fact]
        public void IsCommentOwnerTest()
        {
            var testData = new List<Comment>
            {
                new Comment
                {
                    Id = "testId",
                    OwnerId = "test1",
                    TextContent = "test",
                    PostId = "test1postId",
                },
            };
            var repo = new Mock<IDeletableEntityRepository<Comment>>();
            repo.Setup(r => r.AllAsNoTracking())
                .Returns(testData.AsQueryable);

            var service = new CommentsService(repo.Object);
            var isOwnerTrue = service.IsCommentOwner("test1", "testId");
            var isOwnerFalse1 = service.IsCommentOwner("test2", "testId");
            var isOwnerFalse2 = service.IsCommentOwner("test1", "test");

            Assert.True(isOwnerTrue);
            Assert.False(isOwnerFalse1);
            Assert.False(isOwnerFalse2);
        }

        [Fact]
        public void GetCommentByPostIdTest()
        {
            var testData = new List<Comment>
            {
                new Comment
                {
                    Id = "testId1",
                    OwnerId = "test1",
                    TextContent = "test",
                    PostId = "testPostId1",
                    Owner = new ApplicationUser
                    {
                        UserName = "test",
                        Email = "test@email.com",
                    },
                },
                new Comment
                {
                    Id = "testId2",
                    OwnerId = "test1",
                    TextContent = "test",
                    PostId = "testPostId2",
                    Owner = new ApplicationUser
                    {
                        UserName = "test",
                        Email = "test@email.com",
                    },
                },
                new Comment
                {
                    Id = "testId3",
                    OwnerId = "test2",
                    TextContent = "test",
                    PostId = "testPostId2",
                    Owner = new ApplicationUser
                    {
                        UserName = "test",
                        Email = "test@email.com",
                    },
                },
            };
            var repo = new Mock<IDeletableEntityRepository<Comment>>();
            repo.Setup(r => r.AllAsNoTracking())
                .Returns(testData.AsQueryable);

            var service = new CommentsService(repo.Object);
            var comments = service.GetByPostId("testPostId2");

            Assert.Equal(2, comments.Count);
        }
    }
}
