namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface IBodyStatisticsService
    {
        Task Create(BodyStatisticInputModel input, string userId);

        ICollection<BodyStatisticViewModel> All(string userId);

        Task Delete(string bodyStatisticId);


    }
}
