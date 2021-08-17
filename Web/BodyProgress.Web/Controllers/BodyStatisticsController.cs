using System.Security.Claims;
using System.Threading.Tasks;
using BodyProgress.Services;
using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BodyProgress.Web.Controllers
{
    [Authorize]
    public class BodyStatisticsController : BaseController
    {
        private readonly IBodyStatisticsService bodyStatisticsService;

        public BodyStatisticsController(IBodyStatisticsService bodyStatisticsService)
        {
            this.bodyStatisticsService = bodyStatisticsService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(BodyStatisticInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.bodyStatisticsService.Create(input, userId);

            return this.Redirect("/BodyStatistics/All");
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bodyStatistics = this.bodyStatisticsService.All(userId);

            return this.View(bodyStatistics);
        }

        public async Task<IActionResult> Delete(string bodyStatisticId)
        {
            await this.bodyStatisticsService.Delete(bodyStatisticId);

            return this.Redirect("/BodyStatistics/All");
        }
    }
}
