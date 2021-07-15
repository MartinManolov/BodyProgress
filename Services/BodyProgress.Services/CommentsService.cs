namespace BodyProgress.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task<string> Add(CommentInputModel input, string userId)
        {
            var comment = new Comment()
            {
                OwnerId = userId,
                TextContent = input.TextContent,
                PostId = input.PostId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
            return comment.Id;
        }

        public async Task<bool> Delete(string commentId,string userId)
        {
            var comment = this.commentsRepository.All()
                .FirstOrDefault(x => x.Id == commentId && x.OwnerId == userId);
            if (comment != null)
            {
                this.commentsRepository.Delete(comment);
                await this.commentsRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public ICollection<CommentViewModel> GetByPostId(string postId)
        {
            return this.commentsRepository.AllAsNoTracking()
                .Where(x => x.PostId == postId)
                .OrderBy(x => x.CreatedOn)
                .Select(x => new CommentViewModel
                {
                    Date = x.CreatedOn,
                    Id = x.Id,
                    OwnerName = x.Owner.UserName,
                    TextContent = x.TextContent,
                }).ToList();
        }
    }
}
