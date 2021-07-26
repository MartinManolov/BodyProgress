namespace BodyProgress.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class ProfileBodyStatisticsViewComponent : ViewComponent
    {
        private readonly IBodyStatisticsService bodyStatisticsService;
        private readonly IUsersService usersService;

        public ProfileBodyStatisticsViewComponent(IBodyStatisticsService bodyStatisticsService,
             IUsersService usersService)
        {
            this.bodyStatisticsService = bodyStatisticsService;
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username)
        {
            var userId = this.usersService.GetIdByUsername(username);
            var bodyStatistics = this.bodyStatisticsService.All(userId);
            return this.View(bodyStatistics);
        }
    }
}
