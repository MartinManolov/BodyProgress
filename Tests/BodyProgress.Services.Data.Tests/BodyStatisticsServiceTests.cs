using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using BodyProgress.Services.Common;
using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels.ViewInputModels;
using CloudinaryDotNet;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BodyProgress.Services.Data.Tests
{
    public class BodyStatisticsServiceTests
    {
        [Fact]
        public void CreateBodyStatisticsTest()
        {
            List<BodyStatistic> testData = new List<BodyStatistic>();
            var repo = new Mock<IDeletableEntityRepository<BodyStatistic>>();
            repo.Setup(r => r.All())
            .Returns(testData.AsQueryable);
            repo.Setup(r => r.AddAsync(It.IsAny<BodyStatistic>()))
                .Callback(() => testData.Add(It.IsAny<BodyStatistic>()));

            var userRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();

            IBodyStatisticsService service = new BodyStatisticsService(repo.Object, userRepo.Object);
            var bodyStatisticInputModel = new BodyStatisticInputModel
            {
                BodyFatPercentage = 20,
                Weight = 80,
                Height = 175,
                Date = DateTime.UtcNow,
            };

            service.Create(bodyStatisticInputModel, "testUser1");

            Assert.Single(testData);
        }

        [Fact]
        public void AllBodyStatisticsTest()
        {
            List<BodyStatistic> testData = new List<BodyStatistic>
            {
                new BodyStatistic
                {
                    BodyFatPercentage = 20,
                    Weight = 80,
                    Height = 175,
                    OwnerId = "test1",
                },
                new BodyStatistic
                {
                    BodyFatPercentage = 20,
                    Weight = 80,
                    Height = 175,
                    OwnerId = "test1",
                },
                new BodyStatistic
                {
                    BodyFatPercentage = 20,
                    Weight = 80,
                    Height = 175,
                    OwnerId = "test2",
                },
            };

            var repo = new Mock<IDeletableEntityRepository<BodyStatistic>>();
            repo.Setup(r => r.All())
            .Returns(testData.AsQueryable);

            var userRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            IBodyStatisticsService service = new BodyStatisticsService(repo.Object, userRepo.Object);

            var bodyStatistics = service.All("test1");

            Assert.Equal(2, bodyStatistics.Count);
        }

        [Fact]
        public void DeleteBodyStatisticsTest()
        {
            List<BodyStatistic> testData = new List<BodyStatistic>
            {
                new BodyStatistic
                {
                    Id = "test1",
                    BodyFatPercentage = 20,
                    Weight = 80,
                    Height = 175,
                    OwnerId = "test1",
                },
                new BodyStatistic
                {
                    Id = "test2",
                    BodyFatPercentage = 20,
                    Weight = 80,
                    Height = 175,
                    OwnerId = "test1",
                },
            };

            var repo = new Mock<IDeletableEntityRepository<BodyStatistic>>();
            repo.Setup(r => r.All())
            .Returns(testData.AsQueryable);
            repo.Setup(r => r.Delete(It.IsAny<BodyStatistic>())).
                Callback(() => testData[0].IsDeleted = true);

            var userRepo = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            IBodyStatisticsService service = new BodyStatisticsService(repo.Object, userRepo.Object);

            service.Delete("test1");

            Assert.True(testData[0].IsDeleted);
        }
    }
}
