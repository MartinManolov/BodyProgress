using System.Security.Claims;
using System.Threading.Tasks;
using BodyProgress.Services;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BodyProgress.Web.Controllers
{
    public class BodyStatisticsController : BaseController
    {
        private readonly IBodyStatisticsService _bodyStatisticsService;

        public BodyStatisticsController(IBodyStatisticsService bodyStatisticsService)
        {
            _bodyStatisticsService = bodyStatisticsService;
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddBodyStatisticInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this._bodyStatisticsService.Create(input, userId);

            return this.Redirect("/BodyStatistics/All");
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bodyStatistics = this._bodyStatisticsService.All(userId);

            return this.View(bodyStatistics);
        }

        public async Task<IActionResult> Delete(string bodyStatisticId)
        {
            await this._bodyStatisticsService.Delete(bodyStatisticId);

            return this.Redirect("/BodyStatistics/All");
        }
    }
}
