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
        private readonly IUsersService usersService;

        public PostsController(IPostsService postsService, IUsersService usersService)
        {
            this.postsService = postsService;
            this.usersService = usersService;
        }

        public IActionResult Feeds()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var feeds = this.postsService.AllPublicAndFriends(userId);
            return this.View("~/Views/Posts/Posts.cshtml", feeds);
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

        public IActionResult UserPosts()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var posts = this.postsService.UserPosts(userId);

            return this.View("~/Views/Posts/Posts.cshtml", posts);
        }

        public IActionResult VisitedUserPosts(string visitedUserUsername)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var visitedUserId = this.usersService.GetIdByUsername(visitedUserUsername);
            var posts = this.postsService.VisitedUserPosts(userId, visitedUserId);

            return this.View("~/Views/Posts/Posts.cshtml", posts);
        }

        [HttpGet]
        public IActionResult Change(string postId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.postsService.IsPostOwner(userId, postId))
            {
                return this.Unauthorized();
            }

            this.ViewBag.PostId = postId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Change(PostChangeInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.postsService.IsPostOwner(userId, input.PostId))
            {
                return this.Unauthorized();
            }

            await this.postsService.Change(input);

            return this.Redirect("/Posts/UserPosts");
        }

        public async Task<IActionResult> Delete(string postId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.postsService.IsPostOwner(userId, postId))
            {
                return this.Unauthorized();
            }

            await this.postsService.Delete(postId);

            return this.Redirect("/Posts/UserPosts");
        }

    }
}
