using BodyProgress.Services.Common;
using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BodyProgress.Web.Controllers
{
    [Authorize]
    public class ProfilesController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IUploadMediaService uploadMediaService;

        public ProfilesController(IUsersService usersService,
            IUploadMediaService uploadMediaService)
        {
            this.usersService = usersService;
            this.uploadMediaService = uploadMediaService;
        }

        public IActionResult Info([FromQuery(Name = "username")] string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var visitedUserId = this.usersService.GetIdByUsername(username);
            if (visitedUserId == null)
            {
                return this.NotFound(username);
            }

            if (userId == visitedUserId)
            {
                return this.Redirect("/Profiles/ProfileSettings");
            }

            var user = this.usersService.GetProfileInfo(userId, visitedUserId);

            return this.View(user);
        }

        public IActionResult ProfileSettings()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileSettings = this.usersService.GetProfileSettings(userId);

            return this.View(profileSettings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeVisibility(bool isPublic)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.ChangeProfileVisibility(userId, isPublic);
            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUsername(string username)
        {
            if (username == null || username.Length < 6 || username.Length > 20)
            {
                this.ModelState.AddModelError(username, "Username should be between 6 and 20 character.");
                return this.Redirect("/Profiles/ProfileSettings");
            }

            if (!this.usersService.IsUsernameAvailable(username))
            {
                this.ModelState.AddModelError(username, "Username is already taken.");
                return this.Redirect("/Profiles/ProfileSettings");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.ChangeUsername(userId, username);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePicture(IFormFile image)
        {
            if (image == null)
            {
                return this.Redirect("/Profiles/ProfileSettings");
            }

            if (image.Length > 5 * 1024 * 1024)
            {
                this.ModelState.AddModelError("image", "File must be no larger than 5mb.");
                return this.Redirect("/Profiles/ProfileSettings");
            }

            if (!this.uploadMediaService.IsImage(image))
            {
                this.ModelState.AddModelError("image", "This file type is not allowed.");
                return this.Redirect("/Profiles/ProfileSettings");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.ChangeProfilePicture(userId, image);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        public async Task<IActionResult> RemoveProfilePicture()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.RemoveProfilePicture(userId);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGoal(string goal)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.ChangeGoal(userId, goal);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        public async Task<IActionResult> RemoveGoal()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.RemoveGoal(userId);

            return this.Redirect("/Profiles/ProfileSettings");
        }
    }
}
