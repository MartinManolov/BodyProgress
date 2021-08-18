namespace BodyProgress.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BodyProgress.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FriendsController : BaseController
    {
        private readonly IFriendsService friendsService;
        private readonly IFriendshipsService friendshipsService;
        private readonly IUsersService usersService;

        public FriendsController(IFriendsService friendsService,
            IFriendshipsService friendshipsService,
            IUsersService usersService)
        {
            this.friendsService = friendsService;
            this.friendshipsService = friendshipsService;
            this.usersService = usersService;
        }

        public IActionResult AllFriends()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friends = this.friendsService.AllFriends(userId);

            return this.View(friends);
        }

        public IActionResult ReceivedFriendRequests()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var friendRequests = this.friendsService.ReceivedFriendRequests(userId);

            return this.View(friendRequests);
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
    }
}
