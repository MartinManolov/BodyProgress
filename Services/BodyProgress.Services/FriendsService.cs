namespace BodyProgress.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels;

    public class FriendsService : IFriendsService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Friendship> friendshipsRepository;

        public FriendsService(IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Friendship> friendshipsRepository)
        {
            this.usersRepository = usersRepository;
            this.friendshipsRepository = friendshipsRepository;
        }

        public ICollection<FriendViewModel> AllFriends(string userId)
        {
            var user = this.usersRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            var friendsFriendInitiator = this.friendshipsRepository.AllAsNoTracking()
                .Where(x => x.FriendId == userId && x.Status == Data.Models.Enums.FriendshipStatus.Friends)
                .Select(x => new FriendViewModel()
                {
                    Username = x.User.UserName,
                    ProfilePicture = x.User.ProfilePicture,
                }).ToList();

            var friendsUserInitiator = this.friendshipsRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId && x.Status == Data.Models.Enums.FriendshipStatus.Friends)
                .Select(x => new FriendViewModel()
                {
                    Username = x.Friend.UserName,
                    ProfilePicture = x.Friend.ProfilePicture,
                }).ToList();

            var friends = new List<FriendViewModel>();

            friends.AddRange(friendsFriendInitiator);
            friends.AddRange(friendsUserInitiator);

            return friends;
        }

        public ICollection<FriendViewModel> ReceivedFriendRequests(string userId)
        {
            var user = this.usersRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            var receivedFriendRequests = this.friendshipsRepository.AllAsNoTracking().
                Where(x => x.FriendId == userId && x.Status == Data.Models.Enums.FriendshipStatus.Invited)
                .Select(x => new FriendViewModel()
                {
                    Username = x.User.UserName,
                    ProfilePicture = x.User.ProfilePicture,
                }).ToList();

            return receivedFriendRequests;
        }
    }
}
