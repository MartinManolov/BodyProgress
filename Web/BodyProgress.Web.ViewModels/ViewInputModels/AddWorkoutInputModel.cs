namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Web.ViewModels.ValidationAtributes;

    public class AddWorkoutInputModel
    {
        public AddWorkoutInputModel()
        {
            this.Sets = new List<AddSetInputModel>();
        }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string WorkoutName { get; set; }

        [DateBetween2020andUtsNow]
        public DateTime Date { get; set; }

        public List<AddSetInputModel> Sets { get; set; }
    }
}
