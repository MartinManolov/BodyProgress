using System.Collections.Generic;
using BodyProgress.Web.ViewModels;

namespace BodyProgress.Services
{
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface IBodyStatisticsService
    {
        Task Create(AddBodyStatisticInputModel input, string userId);

        ICollection<BodyStatisticViewModel> All(string userId);

        Task Delete(string bodyStatisticId);


    }
}
