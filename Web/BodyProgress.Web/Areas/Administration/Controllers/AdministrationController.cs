namespace BodyProgress.Web.Areas.Administration.Controllers
{
    using BodyProgress.Common;
    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        private readonly IPostsService postsService;

        public AdministrationController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public IActionResult Feeds()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var feeds = this.postsService.All(userId);

            return this.View("~/Views/Posts/Posts.cshtml", feeds);
        }

        public async Task<IActionResult> DeletePost(string postId)
        {
            await this.postsService.Delete(postId);

            return this.Redirect("/Administration/Administration/Feeds");
        }
    }
}
