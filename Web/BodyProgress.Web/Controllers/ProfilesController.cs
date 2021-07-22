using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BodyProgress.Web.Controllers
{
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

        [Authorize]
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

        public async Task<IActionResult> RemoveFriend([FromQuery(Name = "username")] string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendId = this.usersService.GetIdByUsername(username);
            await this.friendshipsService.RemoveFriend(userId, friendId);
            return this.Redirect($"/Profiles/Info?username={username}");
        }
    }
}
