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
    public class LikesService : ILikesService
    {
        private readonly IDeletableEntityRepository<Like> likesRepository;

        public LikesService(IDeletableEntityRepository<Like> likesRepository)
        {
            this.likesRepository = likesRepository;
        }

        public async Task<string> Add(string userId, string postId)
        {
            var likeDB = this.likesRepository.AllAsNoTracking().FirstOrDefault(x => x.OwnerId == userId && x.PostId == postId);
            if (likeDB != null)
            {
                return null;
            }

            var like = new Like
            {
                OwnerId = userId,
                PostId = postId,
            };

            await this.likesRepository.AddAsync(like);
            await this.likesRepository.SaveChangesAsync();
            return like.Id;
        }

        public async Task<bool> Delete(string userId, string postId)
        {
            var like = this.likesRepository.All().FirstOrDefault(x => x.OwnerId == userId && x.PostId == postId);
            if (like != null)
            {
                this.likesRepository.Delete(like);
                await this.likesRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public int GetByPostId(string postId)
        {
            return this.likesRepository.AllAsNoTracking().Where(x => x.PostId == postId)
                .Count();
        }


    }
}
