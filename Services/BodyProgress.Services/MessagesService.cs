using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using BodyProgress.Services.Contracts;
using BodyProgress.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;
        private readonly IUsersService usersService;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository,
            IUsersService usersService)
        {
            this.messagesRepository = messagesRepository;
            this.usersService = usersService;
        }

        public ICollection<MessageViewModel> AllMessages()
        {
            var messages = this.messagesRepository.AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Take(200)
                .Select(x => new MessageViewModel
                {
                    SenderUsername = x.Sender.UserName,
                    Text = x.Text,
                    Date = x.CreatedOn,
                }).ToList();

            messages.Reverse();

            return messages;
        }

        public async Task SaveMessage(string senderId, string text)
        {
            var sender = this.usersService.GetUsernameById(senderId);
            if (sender == null)
            {
                return;
            }

            var message = new Message
            {
                SenderId = senderId,
                Text = text,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();
        }
    }
}
