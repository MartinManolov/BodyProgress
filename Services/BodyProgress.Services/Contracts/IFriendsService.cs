namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;

    using BodyProgress.Web.ViewModels;

    public interface IFriendsService
    {
        ICollection<FriendViewModel> AllFriends(string userId);

        ICollection<FriendViewModel> ReceivedFriendRequests(string userId);
    }
}
