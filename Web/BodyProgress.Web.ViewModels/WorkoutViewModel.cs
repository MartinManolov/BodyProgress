namespace BodyProgress.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class WorkoutViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public List<SetViewModel> Sets { get; set; }

        public double AllWeightForWorkout { get; set; }
    }
}
