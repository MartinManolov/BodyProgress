namespace BodyProgress.Web.Controllers.Api
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("/api/[controller]")]
    [Authorize]
    public class LikesController : ApiBaseController
    {
        private readonly ILikesService likesService;

        public LikesController(ILikesService likesService)
        {
            this.likesService = likesService;
        }

        [HttpGet]
        public IActionResult GetLikes(string postId)
        {
            var likesCount = this.likesService.GetByPostId(postId);

            return this.Ok(likesCount);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]LikeInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var likeId = await this.likesService.Add(userId, input.PostId);
            if (likeId == null)
            {
                return this.StatusCode(409);
            }

            return this.CreatedAtAction("Add", likeId);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]LikeInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isDeleted = await this.likesService.Delete(userId, input.PostId);
            if (isDeleted)
            {
                return this.StatusCode(204);
            }

            return this.StatusCode(406);
        }
    }
}
