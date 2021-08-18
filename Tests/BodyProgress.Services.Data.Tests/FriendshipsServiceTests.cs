namespace BodyProgress.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using Moq;
    using Xunit;

    public class FriendshipsServiceTests
    {
        [Fact]
        public async Task AddFriendTest()
        {
            var testUsersData = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "testId1",
                    UserName = "test1",
                    Email = "test1@email.com",
                },
                new ApplicationUser
                {
                    Id = "testId2",
                    UserName = "test2",
                    Email = "test2@email.com",
                },
                new ApplicationUser
                {
                    Id = "testId3",
                    UserName = "test3",
                    Email = "test3@email.com",
                },
            };

            var testFriendshipsData = new List<Friendship>();

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            usersRepo.Setup(r => r.All())
                .Returns(testUsersData.AsQueryable);
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.AllWithDeleted())
                .Returns(testFriendshipsData.AsQueryable);
            friendshipsRepo.Setup(r => r.AddAsync(It.IsAny<Friendship>()))
                .Callback(() => testFriendshipsData.Add(It.IsAny<Friendship>()));

            var service = new FriendshipsService(friendshipsRepo.Object, usersRepo.Object);
            await service.AddFriend("testId1", "testId2");

            Assert.Single(testFriendshipsData);
        }

        [Fact]
        public async Task AcceptFriendTest()
        {
            var testUsersData = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "testId1",
                    UserName = "test1",
                    Email = "test1@email.com",
                },
                new ApplicationUser
                {
                    Id = "testId2",
                    UserName = "test2",
                    Email = "test2@email.com",
                },
                new ApplicationUser
                {
                    Id = "testId3",
                    UserName = "test3",
                    Email = "test3@email.com",
                },
            };

            var testFriendshipsData = new List<Friendship>
            {
                new Friendship
                {
                    UserId = "testId1",
                    FriendId = "testId2",
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Invited,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            usersRepo.Setup(r => r.All())
                .Returns(testUsersData.AsQueryable);
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.All())
                .Returns(testFriendshipsData.AsQueryable);

            var service = new FriendshipsService(friendshipsRepo.Object, usersRepo.Object);
            await service.AcceptFriend("testId2", "testId1");

            Assert.Equal(BodyProgress.Data.Models.Enums.FriendshipStatus.Friends, testFriendshipsData[0].Status);
        }

        [Fact]
        public async Task RemoveFriendTest()
        {
            var testUsersData = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "testId1",
                    UserName = "test1",
                    Email = "test1@email.com",
                },
                new ApplicationUser
                {
                    Id = "testId2",
                    UserName = "test2",
                    Email = "test2@email.com",
                },
                new ApplicationUser
                {
                    Id = "testId3",
                    UserName = "test3",
                    Email = "test3@email.com",
                },
            };

            var testFriendshipsData = new List<Friendship>
            {
                new Friendship
                {
                    UserId = "testId1",
                    FriendId = "testId2",
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Friends,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            usersRepo.Setup(r => r.All())
                .Returns(testUsersData.AsQueryable);
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.All())
                .Returns(testFriendshipsData.AsQueryable);
            friendshipsRepo.Setup(r => r.Delete(It.IsAny<Friendship>()))
                .Callback(() => testFriendshipsData[0].IsDeleted = true);

            var service = new FriendshipsService(friendshipsRepo.Object, usersRepo.Object);
            await service.RemoveFriend("testId2", "testId1");

            Assert.True(testFriendshipsData[0].IsDeleted);
        }

        [Fact]
        public void IsFriendTest()
        {
            var testFriendshipsData = new List<Friendship>
            {
                new Friendship
                {
                    UserId = "testId1",
                    FriendId = "testId2",
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Friends,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.AllAsNoTracking())
                 .Returns(testFriendshipsData.AsQueryable);
            var service = new FriendshipsService(friendshipsRepo.Object, usersRepo.Object);
            bool isFriendTrue = service.IsFriend("testId2", "testId1");
            bool isFriendFalse = service.IsFriend("testId3", "testId1");

            Assert.True(isFriendTrue);
            Assert.False(isFriendFalse);
        }

        [Fact]
        public void IsReceivedRequestTest()
        {
            var testFriendshipsData = new List<Friendship>
            {
                new Friendship
                {
                    UserId = "testId1",
                    FriendId = "testId2",
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Invited,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.AllAsNoTracking())
                 .Returns(testFriendshipsData.AsQueryable);
            var service = new FriendshipsService(friendshipsRepo.Object, usersRepo.Object);
            bool isReceivedRequestTrue = service.IsReceivedRequest("testId2", "testId1");
            bool isReceivedRequestFalse = service.IsReceivedRequest("testId1", "testId2");

            Assert.True(isReceivedRequestTrue);
            Assert.False(isReceivedRequestFalse);
        }

        [Fact]
        public void IsSendedRequestTest()
        {
            var testFriendshipsData = new List<Friendship>
            {
                new Friendship
                {
                    UserId = "testId1",
                    FriendId = "testId2",
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Invited,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.AllAsNoTracking())
                 .Returns(testFriendshipsData.AsQueryable);
            var service = new FriendshipsService(friendshipsRepo.Object, usersRepo.Object);
            bool isReceivedRequestFalse = service.IsSendedRequest("testId2", "testId1");
            bool isReceivedRequestTrue = service.IsSendedRequest("testId1", "testId2");

            Assert.True(isReceivedRequestTrue);
            Assert.False(isReceivedRequestFalse);
        }
    }
}
