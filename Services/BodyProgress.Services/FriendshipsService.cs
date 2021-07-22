
using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using BodyProgress.Data.Models.Enums;
using BodyProgress.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyProgress.Services
{
    public class FriendshipsService : IFriendshipsService
    {
        private readonly IDeletableEntityRepository<Friendship> friendhipsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public FriendshipsService(IDeletableEntityRepository<Friendship> friendhipsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.friendhipsRepository = friendhipsRepository;
            this.usersRepository = usersRepository;
        }

        public async Task AddFriend(string userId, string friendId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var friend = this.usersRepository.All().FirstOrDefault(x => x.Id == friendId);
            if (user == null || friend == null)
            {
                return;
            }

            var userFriendship = this.friendhipsRepository.AllWithDeleted().FirstOrDefault(x => x.UserId == userId && x.FriendId == friendId);
            if (userFriendship != null)
            {
                if (userFriendship.IsDeleted)
                {
                    userFriendship.IsDeleted = false;
                    userFriendship.Status = FriendshipStatus.Invited;
                    await this.friendhipsRepository.SaveChangesAsync();
                    return;
                }
                else if (userFriendship.Status == Enum.Parse<FriendshipStatus>("Friends"))
                {
                    return;
                }
                else if (userFriendship.Status == Enum.Parse<FriendshipStatus>("Invited"))
                {
                    return;
                }
            }

            userFriendship = new Friendship()
            {
                UserId = userId,
                FriendId = friendId,
                Status = FriendshipStatus.Invited,
            };

            await this.friendhipsRepository.AddAsync(userFriendship);

            await this.friendhipsRepository.SaveChangesAsync();
            return;
        }

        public async Task AcceptFriend (string userId, string friendId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var friend = this.usersRepository.All().FirstOrDefault(x => x.Id == friendId);
            if (user == null || friend == null)
            {
                return;
            }

            var friendship = this.friendhipsRepository.All().
                FirstOrDefault(x => x.UserId == friendId && x.FriendId == userId && x.Status == FriendshipStatus.Invited);

            if (friendship == null)
            {
                return;
            }

            friendship.Status = FriendshipStatus.Friends;
            await this.friendhipsRepository.SaveChangesAsync();
        }

        public async Task RemoveFriend(string userId, string friendId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var friend = this.usersRepository.All().FirstOrDefault(x => x.Id == friendId);
            if (user == null || friend == null)
            {
                return;
            }

            var userFriendship = this.friendhipsRepository.All()
                .FirstOrDefault(x => x.UserId == userId && x.FriendId == friendId);
            var friendFriendship = this.friendhipsRepository.All()
                    .FirstOrDefault(x => x.UserId == friendId && x.FriendId == userId);

            if (userFriendship != null)
            {
                userFriendship.Status = FriendshipStatus.NonAccepted;
                this.friendhipsRepository.Delete(userFriendship);
            }

            if (friendFriendship != null)
            {
                friendFriendship.Status = FriendshipStatus.NonAccepted;
                this.friendhipsRepository.Delete(friendFriendship);
            }

            await this.friendhipsRepository.SaveChangesAsync();
        }

        public bool IsFriend(string userId, string friendId)
        {
            return this.friendhipsRepository.AllAsNoTracking()
                .Any(x => (x.UserId == userId && x.FriendId == friendId && x.Status == FriendshipStatus.Friends) ||
                (x.UserId == friendId && x.FriendId == userId && x.Status == FriendshipStatus.Friends));
        }

        public bool IsReceivedRequest(string userId, string friendId)
        {
            return this.friendhipsRepository.AllAsNoTracking()
                .Any(x => x.UserId == friendId && x.FriendId == userId && x.Status == FriendshipStatus.Invited);
        }

        public bool IsSendedRequest(string userId, string friendId)
        {
            return this.friendhipsRepository.AllAsNoTracking()
                .Any(x => x.UserId == userId && x.FriendId == friendId && x.Status == FriendshipStatus.Invited);
        }
    }
}
