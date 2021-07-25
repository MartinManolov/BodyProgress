using BodyProgress.Web.ViewModels;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BodyProgress.Services.Contracts
{
    public interface IUsersService
    {
        string GetUsernameById(string userId);

        string GetIdByUsername(string username);

        bool IsPublic(string userId);

        ProfileViewModel GetProfileInfo(string userId, string visitedUserId);

        ProfileSettingsViewModel GetProfileSettings(string userId);

        string GetProfileImage(string userId);

        string GetGoal(string userId);

        Task ChangeProfilePicture(string userId, IFormFile input);

        Task RemoveProfilePicture(string userId);

        Task RemoveGoal(string userId);

        Task ChangeUsername(string userId, string newUsername);

        Task ChangeProfileVisibility(string userId, bool isPublic);

        Task ChangeGoal(string userId, string goal);

        bool IsUsernameAvailable(string username);
    }
}
