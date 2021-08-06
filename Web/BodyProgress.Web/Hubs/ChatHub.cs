using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BodyProgress.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessagesService messagesService;

        public ChatHub(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task SendToAll(string message)
        {
            if (message.Length == 0)
            {
                return;
            }

            var userId = this.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usernameSender = this.Context.User.Identity.Name;

            await this.messagesService.SaveMessage(userId, message);

            await this.Clients.All.SendAsync("NewMessage", new MessageViewModel { SenderUsername = usernameSender, Text = message, Date = System.DateTime.UtcNow });
        }
    }
}
