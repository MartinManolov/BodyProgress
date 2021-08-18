namespace BodyProgress.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealsController : BaseController
    {
        private readonly IMealsService mealsService;

        public MealsController(IMealsService mealsService)
        {
            this.mealsService = mealsService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MealInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.mealsService.Create(input, userId);
            return this.Redirect("/Meals/All");
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meals = this.mealsService.All(userId);
            return this.View(meals);
        }

        public async Task<IActionResult> Delete(string mealId)
        {
            await this.mealsService.Delete(mealId);
            return this.Redirect("/Meals/All");
        }
    }
}
