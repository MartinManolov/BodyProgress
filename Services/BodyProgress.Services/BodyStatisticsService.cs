namespace BodyProgress.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public class BodyStatisticsService : IBodyStatisticsService
    {
        private readonly IDeletableEntityRepository<BodyStatistic> bodyStatisticRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public BodyStatisticsService(IDeletableEntityRepository<BodyStatistic> bodyStatisticRepository,
                                        IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.bodyStatisticRepository = bodyStatisticRepository;
            this.userRepository = userRepository;
        }

        public async Task Create(BodyStatisticInputModel input, string userId)
        {
            var bodyStatistic = new BodyStatistic()
            {
                OwnerId = userId,
                Date = input.Date,
                Height = input.Height,
                Weight = input.Weight,
                BodyFatPercentage = input.BodyFatPercentage,
            };

            await this.bodyStatisticRepository.AddAsync(bodyStatistic);
            var user = this.userRepository.All().FirstOrDefault(x => x.Id == userId);
            user.BodyStatistics.Add(bodyStatistic);

            await this.bodyStatisticRepository.SaveChangesAsync();
            await this.userRepository.SaveChangesAsync();
        }

        public ICollection<BodyStatisticViewModel> All(string userId)
        {
            return this.bodyStatisticRepository.All()
                .Where(x => x.OwnerId == userId)
                .Select(x => new BodyStatisticViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    BodyFatPercentage = x.BodyFatPercentage,
                    Height = x.Height,
                    Weight = x.Weight,
                }).OrderByDescending(x => x.Date)
                .ToList();
        }

        public async Task Delete(string bodyStatisticId)
        {
            var bodyStatistic = this.bodyStatisticRepository.All().FirstOrDefault(x => x.Id == bodyStatisticId);
            if (bodyStatistic == null)
            {
                return;
            }

            this.bodyStatisticRepository.Delete(bodyStatistic);
            await this.bodyStatisticRepository.SaveChangesAsync();
        }
    }
}
