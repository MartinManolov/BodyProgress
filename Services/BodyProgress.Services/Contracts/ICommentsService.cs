namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface ICommentsService
    {
        Task<string> Add(CommentInputModel input, string userId);

        Task<bool> Delete(string commentId, string userId);

        ICollection<CommentViewModel> GetByPostId(string postId);
    }
}
