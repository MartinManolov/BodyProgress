namespace BodyProgress.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface IMealsService
    {
        Task Create(MealInputModel input, string userId);

        ICollection<MealViewModel> All(string userId);

        Task Delete(string mealId);
    }
}
