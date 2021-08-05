using BodyProgress.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BodyProgress.Web.Controllers
{
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
