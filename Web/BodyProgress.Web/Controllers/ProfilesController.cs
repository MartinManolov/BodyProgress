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
            var userId = this.usersService.GetIdByUsername(username);
            if (userId == null)
            {
                return this.NotFound(username);
            }

            var usernameModel = new UsernameViewModel { Username = username };

            return this.View(usernameModel);
        }

        public async Task<IActionResult> AddFriend(string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendId = this.usersService.GetIdByUsername(username);
            await this.friendshipsService.AddFriend(userId, friendId);
            return this.Redirect($"/Profiles/Info?username={username}");
        }
    }
}
