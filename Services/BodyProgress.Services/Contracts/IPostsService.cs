using System.Collections.Generic;
using BodyProgress.Web.ViewModels;

namespace BodyProgress.Services.Contracts
{
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface IPostsService
    {
        Task Create(PostInputModel input, string userId);

        Task Delete(string postId);

        ICollection<PostViewModel> AllPublicAndFriends(string userId);
    }
}
