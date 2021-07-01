namespace BodyProgress.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class WorkoutsController : BaseController
    {
        private readonly IWorkoutsService _workoutsService;

        public WorkoutsController(IWorkoutsService workoutsService)
        {
            this._workoutsService = workoutsService;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddWorkoutInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._workoutsService.Create(input, userId);

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workouts = this._workoutsService.All(userId);
            ;
            return this.View(workouts);
        }
    }
}
