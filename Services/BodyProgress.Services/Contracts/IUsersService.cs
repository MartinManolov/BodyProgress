namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;
    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        ICollection<string> Search(string expression, int count);

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
