namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;

    public interface IMessagesService
    {
        Task SaveMessage(string senderId, string text);

        ICollection<MessageViewModel> AllMessages();
    }
}
