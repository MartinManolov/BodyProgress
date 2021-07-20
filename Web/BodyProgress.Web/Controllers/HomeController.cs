using System.Linq;
using BodyProgress.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace BodyProgress.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using BodyProgress.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IPostsService _postsService;

        public HomeController(IPostsService postsService)
        {
            this._postsService = postsService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Home/Feed");
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Feed()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var feeds = this._postsService.AllPublicAndFriends(userId);
            return this.View(feeds.ToList());
        }
    }
}
