namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;

    public interface ILikesService
    {
        public int GetByPostId(string postId);

        public Task<string> Add(string userId, string postId);

        public Task<bool> Delete(string userId, string postId);
    }
}
