namespace BodyProgress.Services
{
    using System.Linq;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Contracts;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public string GetIdByUsername(string username)
        {
            var user = this.usersRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public bool IsPublic(string userId)
        {
            return this.usersRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == userId).IsPublic;
        }
    }
}
