using BodyProgress.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Services.Contracts
{
    public interface IFriendsService
    {
        ICollection<FriendViewModel> AllFriends(string userId);

        ICollection<FriendViewModel> ReceivedFriendRequests(string userId);
    }
}
