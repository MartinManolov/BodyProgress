namespace BodyProgress.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class WorkoutsController : BaseController
    {
        private readonly IWorkoutsService workoutsService;
        private readonly IUsersService usersService;

        public WorkoutsController(IWorkoutsService workoutsService,
            IUsersService usersService)
        {
            this.workoutsService = workoutsService;
            this.usersService = usersService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(WorkoutInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.workoutsService.Create(input, userId);

            return this.RedirectToAction("All", "Workouts");
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workouts = this.workoutsService.All(userId);
            return this.View(workouts);
        }

        public async Task<IActionResult> Delete(string workoutId)
        {
            await this.workoutsService.Delete(workoutId);
            return this.Redirect("/Workouts/All");
        }
    }
}
