namespace BodyProgress.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("/api/[controller]")]
    [Authorize]
    public class CommentsController : ApiBaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpGet]
        public JsonResult GetAll(string postId)
        {
            var comments = this.commentsService.GetByPostId(postId);

            return this.Json(comments.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CommentInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var commentId = await this.commentsService.Add(input, userId);

            return this.CreatedAtAction("Add", commentId);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string commentId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isDeleted = await this.commentsService.Delete(commentId, userId);
            if (isDeleted)
            {
                return this.StatusCode(200);
            }

            return this.StatusCode(204);
        }
    }
}
