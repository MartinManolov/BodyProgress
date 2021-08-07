namespace BodyProgress.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class ProfileWorkoutsViewComponent : ViewComponent
    {
        private readonly IWorkoutsService workoutsService;
        private readonly IUsersService usersService;

        public ProfileWorkoutsViewComponent(IWorkoutsService workoutsService,
            IUsersService usersService)
        {
            this.workoutsService = workoutsService;
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username)
        {
            var userId = this.usersService.GetIdByUsername(username);
            var workouts = this.workoutsService.All(userId);
            return this.View(workouts);
        }
    }
}
