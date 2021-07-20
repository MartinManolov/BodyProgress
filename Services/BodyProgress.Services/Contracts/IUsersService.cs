using System.Threading.Tasks;

namespace BodyProgress.Services.Contracts
{
    public interface IUsersService
    {
        string GetIdByUsername(string userId);
    }
}
