namespace BodyProgress.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IFriendshipsService
    {
        Task AddFriend(string userId, string friendId);

        Task AcceptFriend(string userId, string friendId);

        Task RemoveFriend(string userId, string friendId);

        bool IsFriend(string userId, string friendId);

        bool IsReceivedRequest(string userId, string friendId);

        bool IsSendedRequest(string userId, string friendId);
    }
}
