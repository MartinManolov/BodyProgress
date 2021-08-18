namespace BodyProgress.Web.Controllers
{
    using BodyProgress.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ChatsController : BaseController
    {
        private readonly IMessagesService messagesService;

        public ChatsController(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public IActionResult ChatBox()
        {
            var messages = this.messagesService.AllMessages();
            return this.View(messages);
        }
    }
}
