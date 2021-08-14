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
    public class FriendsServiceTests
    {
        [Fact]
        public void AllFriendsTest()
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
                    User = testUsersData[0],
                    FriendId = "testId2",
                    Friend = testUsersData[1],
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Friends,
                },
                new Friendship
                {
                    UserId = "testId3",
                    User = testUsersData[2],
                    FriendId = "testId1",
                    Friend = testUsersData[0],
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Friends,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            usersRepo.Setup(r => r.AllAsNoTracking())
                .Returns(testUsersData.AsQueryable);
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.AllAsNoTracking())
                .Returns(testFriendshipsData.AsQueryable);

            var service = new FriendsService(usersRepo.Object, friendshipsRepo.Object);
            var friends = service.AllFriends("testId1");

            Assert.Equal(2, friends.Count);
        }

        [Fact]
        public void ReceivedFriendRequestTest()
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
                    User = testUsersData[0],
                    FriendId = "testId2",
                    Friend = testUsersData[1],
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Invited,
                },
                new Friendship
                {
                    UserId = "testId3",
                    User = testUsersData[2],
                    FriendId = "testId1",
                    Friend = testUsersData[0],
                    Status = BodyProgress.Data.Models.Enums.FriendshipStatus.Invited,
                },
            };

            var usersRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            usersRepo.Setup(r => r.AllAsNoTracking())
                .Returns(testUsersData.AsQueryable);
            var friendshipsRepo = new Mock<IDeletableEntityRepository<Friendship>>();
            friendshipsRepo.Setup(r => r.AllAsNoTracking())
                .Returns(testFriendshipsData.AsQueryable);

            var service = new FriendsService(usersRepo.Object, friendshipsRepo.Object);
            var friends = service.ReceivedFriendRequests("testId1");

            Assert.Single(friends);
        }
    }
}
