using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BodyProgress.Web.Controllers
{
    [Authorize]
    public class PostsController : BaseController
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.postsService.Create(input, userId);

            return this.Redirect("/");
        }

    }
}
