namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface IWorkoutsService
    {
        Task Create(WorkoutInputModel workout, string userId);

        ICollection<WorkoutViewModel> All(string userId);

        Task Delete(string workoutId);
    }
}
