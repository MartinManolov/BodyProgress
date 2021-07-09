using BodyProgress.Services.Contracts;

namespace BodyProgress.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class MealsController : BaseController
    {
        private readonly IMealsService _mealsService;

        public MealsController(IMealsService mealsService)
        {
            this._mealsService = mealsService;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(MealInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._mealsService.Create(input, userId);
            return this.Redirect("/");

        }

        [Authorize]
        public IActionResult All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meals = this._mealsService.All(userId);
            return this.View(meals);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string mealId)
        {
            await this._mealsService.Delete(mealId);
            return this.Redirect("/Meals/All");
        }

    }
}
