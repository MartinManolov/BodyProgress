namespace BodyProgress.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Contracts;

    public class LikesService : ILikesService
    {
        private readonly IDeletableEntityRepository<Like> likesRepository;

        public LikesService(IDeletableEntityRepository<Like> likesRepository)
        {
            this.likesRepository = likesRepository;
        }

        public async Task<string> Add(string userId, string postId)
        {
            var likeDB = this.likesRepository.AllWithDeleted().FirstOrDefault(x => x.OwnerId == userId && x.PostId == postId);
            if (likeDB != null)
            {
                if (likeDB.IsDeleted == true)
                {
                    likeDB.IsDeleted = false;
                    await this.likesRepository.SaveChangesAsync();
                    return likeDB.Id;
                }

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
