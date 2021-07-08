using System.Collections.Generic;
using System.Linq;
using BodyProgress.Web.ViewModels;

namespace BodyProgress.Services
{
    using System;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public class BodyStatisticsService : IBodyStatisticsService
    {
        private readonly IDeletableEntityRepository<BodyStatistic> _bodyStatisticRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;

        public BodyStatisticsService(IDeletableEntityRepository<BodyStatistic> bodyStatisticRepository,
                                        IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this._bodyStatisticRepository = bodyStatisticRepository;
            this._userRepository = userRepository;
        }

        public async Task Create(AddBodyStatisticInputModel input, string userId)
        {
            var bodyStatistic = new BodyStatistic()
            {
                OwnerId = userId,
                Date = input.Date,
                Height = input.Height,
                Weight = input.Weight,
                BodyFatPercentage = input.BodyFatPercentage,
            };

            await this._bodyStatisticRepository.AddAsync(bodyStatistic);
            var user = this._userRepository.All().FirstOrDefault(x => x.Id == userId);
            user.BodyStatistics.Add(bodyStatistic);

            await this._bodyStatisticRepository.SaveChangesAsync();
            await this._userRepository.SaveChangesAsync();
        }

        public ICollection<BodyStatisticViewModel> All(string userId)
        {
            return this._bodyStatisticRepository.All()
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
            var bodyStatistic = this._bodyStatisticRepository.All().FirstOrDefault(x => x.Id == bodyStatisticId);
            if (bodyStatistic == null)
            {
                return;
            }

            this._bodyStatisticRepository.Delete(bodyStatistic);
            await this._bodyStatisticRepository.SaveChangesAsync();
        }
    }
}
