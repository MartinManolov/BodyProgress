using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Threading.Tasks;

namespace BodyProgress.Web.Controllers
{
    [Authorize]
    public class ProfilesController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IFriendshipsService friendshipsService;

        public ProfilesController(IUsersService usersService,
            IFriendshipsService friendshipsService)
        {
            this.usersService = usersService;
            this.friendshipsService = friendshipsService;
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
                return this.Redirect("/Home/Feed");
            }

            var usernameModel = new ProfileViewModel
            {
                Username = username,
                IsPublic = this.usersService.IsPublic(visitedUserId),
                IsFriend = this.friendshipsService.IsFriend(userId, visitedUserId),
                IsReceivedRequest = this.friendshipsService.IsReceivedRequest(userId, visitedUserId),
                IsSendedRequest = this.friendshipsService.IsSendedRequest(userId, visitedUserId),
            };

            return this.View(usernameModel);
        }

        public async Task<IActionResult> AddFriend([FromQuery(Name = "username")] string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendId = this.usersService.GetIdByUsername(username);
            await this.friendshipsService.AddFriend(userId, friendId);
            return this.Redirect($"/Profiles/Info?username={username}");
        }

        public async Task<IActionResult> AcceptFriend([FromQuery(Name = "username")] string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendId = this.usersService.GetIdByUsername(username);
            await this.friendshipsService.AcceptFriend(userId, friendId);
            return this.Redirect($"/Profiles/Info?username={username}");
        }

        public async Task<IActionResult> RemoveFriend([FromQuery(Name = "username")] string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendId = this.usersService.GetIdByUsername(username);
            await this.friendshipsService.RemoveFriend(userId, friendId);
            return this.Redirect($"/Profiles/Info?username={username}");
        }

        [HttpGet]
        public IActionResult ProfileSettings()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profilePic = this.usersService.GetProfileImage(userId);
            var visibility = "Not Public (Only friends)";
            if (this.usersService.IsPublic(userId))
            {
                visibility = "Public";
            }

            var profileSettings = new ProfileSettingsViewModel()
                {
                Visibility = visibility,
                ProfilePicture = profilePic,
                Username = this.usersService.GetUsernameById(userId),
                Goal = this.usersService.GetGoal(userId),
                };

            return this.View(profileSettings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeVisibility([FromBody]bool isPublic)
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
            }

            if (!this.usersService.IsUsernameAvailable(username))
            {
                this.ModelState.AddModelError(username, "Username is already taken.");
            }
            else
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await this.usersService.ChangeUsername(userId, username);
            }

            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePicture(ProfilePictureInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.ChangeProfilePicture(userId, input);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.RemoveProfilePicture(userId);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGoal([FromBody]string goal)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.ChangeGoal(userId, goal);

            return this.Redirect("/Profiles/ProfileSettings");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveGoal()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.usersService.RemoveGoal(userId);

            return this.Redirect("/Profiles/ProfileSettings");
        }
    }
}
