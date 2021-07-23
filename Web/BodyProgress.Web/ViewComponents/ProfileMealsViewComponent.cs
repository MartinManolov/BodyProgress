namespace BodyProgress.Web.ViewComponents
{
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class ProfileMealsViewComponent : ViewComponent
    {
        private readonly IMealsService mealsService;
        private readonly IUsersService usersService;

        public ProfileMealsViewComponent(IMealsService mealsService,
            IUsersService usersService)
        {
            this.mealsService = mealsService;
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username)
        {
            var userId = this.usersService.GetIdByUsername(username);
            var meals = this.mealsService.All(userId);
            return this.View(meals);
        }
    }
}
