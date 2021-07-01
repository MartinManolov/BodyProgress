﻿using System.Collections.Generic;
using BodyProgress.Web.ViewModels;

namespace BodyProgress.Services
{
    using System.Threading.Tasks;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public interface IWorkoutsService
    {
        Task Create(AddWorkoutInputModel workout, string userId);

        ICollection<WorkoutViewModel> All(string userId);
    }
}
