namespace BodyProgress.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Common;
    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels.ViewInputModels;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IUploadMediaService uploadMediaService;
        private readonly Cloudinary cloudinary;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository,
            IUploadMediaService uploadMediaService,
            Cloudinary cloudinary)
        {
            this.usersRepository = usersRepository;
            this.uploadMediaService = uploadMediaService;
            this.cloudinary = cloudinary;
        }

        public async Task ChangeGoal(string userId, string goal)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return;
            }

            user.Goal = goal;
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task RemoveGoal(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return;
            }

            user.Goal = null;
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task ChangeProfilePicture(string userId, IFormFile input)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return;
            }

            var imageName = userId + "ProfilePicture_" + Guid.NewGuid().ToString();
            var imageUrl = await this.uploadMediaService.UploadImage(this.cloudinary, input, imageName);

            user.ProfilePicture = imageUrl;
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task RemoveProfilePicture(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return;
            }

            user.ProfilePicture = null;
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task ChangeProfileVisibility(string userId, bool isPublic)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return;
            }

            user.IsPublic = isPublic;
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task ChangeUsername(string userId, string newUsername)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return;
            }

            user.UserName = newUsername;
            await this.usersRepository.SaveChangesAsync();
        }

        public string GetGoal(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            return user.Goal;
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

        public string GetProfileImage(string userId)
        {
            var user = this.usersRepository.AllAsNoTracking()
               .FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            return user.ProfilePicture;
            }

        public string GetUsernameById(string userId)
        {
            var user = this.usersRepository.AllAsNoTracking()
                .FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            return user.UserName;
        }

        public bool IsPublic(string userId)
        {
            return this.usersRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == userId).IsPublic;
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.usersRepository.AllAsNoTracking().Any(x => x.UserName == username);
        }
    }
}
